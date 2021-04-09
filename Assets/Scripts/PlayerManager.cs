using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // UI element shown when all players are ready
    public GameObject startGameHint;

    // static list of all players, list is accesible after the game is started
    public static List<PlayerProperties> players;

    public PlayerCard[] cards;

    PlayerControls input;

    public int maxPlayerCunt;

    Color[] defaultColors = {Color.blue, Color.red, Color.green, Color.yellow };

    private void Awake()
    {
        players = new List<PlayerProperties>();
        input = new PlayerControls();
        input.PlayerConnection.Choose.performed += ChooseButtonPressed;
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change == InputDeviceChange.Disconnected)
            {
                RemovePlyer(device);
            }
        };
    }

    private void Start()
    {
        foreach (PlayerCard card in cards)
        {
            card.postReadyCallback += StartGameCallback;
            card.stateChanged += stateChangeCallback;
        }
        startGameHint.SetActive(false);
    }

    private bool ReadyCheck()
    {
        int readyCount = 0;
        foreach (PlayerCard card in cards)
        {
            if (card.CardState == PlayerCard.state.customization)
            {
                return false;
            }
            else if (card.CardState == PlayerCard.state.ready)
            {
                readyCount++;
            }
        }
        if (readyCount > 1)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    private void StartGameCallback(PlayerCard sender)
    {
        if (ReadyCheck())
        {
            players = new List<PlayerProperties>();
            foreach (PlayerCard card in cards)
            {
                if (card.Player != null)
                {
                    players.Add(card.Player);
                }
            }

            gameObject.SetActive(false);
            startGameHint.SetActive(false);

            Debug.Log("Starting game");
            // TODO: Start Game
            GameController.instance.StartGame();

        }
    }

    private void stateChangeCallback(PlayerCard sender)
    {
        if(gameObject.activeInHierarchy)
            startGameHint.SetActive(ReadyCheck());
    }

    /// <summary>
    /// Assigning player to input device that triggered event
    /// </summary>
    /// <param name="ctx">Context parameter of input</param>
    private void ChooseButtonPressed(InputAction.CallbackContext ctx) {
        // checking if device is assigned
        foreach (PlayerCard card in cards)
        {
            if(card.Player!=null && card.Player.Device == ctx.control.device)
            {
                return;
            }
        }
        AddPlayer(ctx.control.device);
    }

    private void OnEnable()
    {
        input.PlayerConnection.Enable();
    }

    private void OnDisable()
    {
        input.PlayerConnection.Disable();
    }

    /// <summary>
    /// Adds player with specific device to a pool and notifies a player card
    /// </summary>
    /// <param name="device"></param>
    private void AddPlayer(InputDevice device) {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].CardState == PlayerCard.state.waiting)
            {
                PlayerProperties newPlayer = new PlayerProperties(i, device, defaultColors[i]);
                cards[i].SetPlayer(newPlayer);
                return;
            }
        }
        Debug.Log("No room for player");
    }

    /// <summary>
    /// Removes player with specific device from a pool and notifies a player card
    /// </summary>
    /// <param name="device"></param>
    private void RemovePlyer(InputDevice device) {
        foreach (PlayerCard card in cards)
        {
            if (card.Player?.Device == device) {
                card.RemovePlayer();
            }
        }
    }
}

/// <summary>
/// Class containing all necessary properties of a player
/// </summary>
public class PlayerProperties {
    public int Id {
        private set;
        get;
    }

    public InputDevice Device
    {
        private set;
        get;
    }

    public Color Color
    {
        private set;
        get;
    }

    public PlayerProperties(int id, InputDevice device, Color color) {
        this.Id = id;
        this.Device = device;
        this.Color = color;
    }
}
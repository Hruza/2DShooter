using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerCard : MonoBehaviour
{
    // UI elements that are changed when a state is changed
    public GameObject pJoining;
    public GameObject pCustomization;
    public GameObject iReady;

    private PlayerProperties player;
    public PlayerProperties Player {
        get {
            return player;
        }
    }

    // Callback events 
    public event Action<PlayerCard> postReadyCallback;
    public event Action<PlayerCard> stateChanged;

    public enum state {waiting, customization, ready}

    private state cardState;
    /// <summary>
    /// Current state of the card:
    /// waiting: no player assigned
    /// customization: player is assigned and is being customized
    /// ready: player customized and ready to play
    /// </summary>
    public state CardState
    {
        get
        {
            return cardState;
        }
        private set
        {
            switch (value)
            {
                case state.waiting:
                    SetDevice(null);
                    player = null;

                    iReady.SetActive(false);
                    pCustomization.SetActive(false);
                    pJoining.SetActive(true);
                    break;

                case state.customization:
                    iReady.SetActive(false);
                    pCustomization.SetActive(true);
                    pJoining.SetActive(false);
                    break;

                case state.ready:
                    switch (cardState)
                    {
                        case state.waiting:
                            Debug.LogError("Trying to be ready without joining");
                            break;
                        case state.ready:
                            postReadyCallback?.Invoke(this);
                            break;
                    }
                    // set ready visuals
                    iReady.SetActive(true);
                    break;
            }
            cardState = value;
            stateChanged?.Invoke(this);
        }
    }

    PlayerControls input;

    private void Awake()
    {
        input = new PlayerControls();
    }

    private void SetDevice(InputDevice device)
    {
        if (device == null)
            input.PlayerConnection.Disable();
        else
        {
            // start input and assign device
            input = new PlayerControls();
            input.PlayerConnection.Enable();
            InputDevice[] deviceArray= new InputDevice[1];
            deviceArray[0] = device;
            input.devices = new ReadOnlyArray<InputDevice>(deviceArray);

            //TODO: send player info to customization classes
            input.PlayerConnection.Choose.performed += (ctx) => { CardState = state.ready;  };
            input.PlayerConnection.Back.performed += (ctx) => {
                if (CardState == state.ready) {
                    CardState = state.customization;
                }
                else if (CardState == state.customization)
                {
                    CardState = state.waiting;
                }
            };
        }
    }

    public void Start()
    {
        CardState = state.waiting;
    }

    public void SetPlayer(PlayerProperties p) {
        player = p;
        SetDevice(p.Device);
        CardState = state.customization;
    }

    public void RemovePlayer() {
        player = null;
        CardState = state.waiting;
    }

    public void OnDisable()
    {
        input.PlayerConnection.Disable();
    }
}

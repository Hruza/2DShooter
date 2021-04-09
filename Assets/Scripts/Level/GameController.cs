using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    private List<GameObject> players;

    public GameObject playerPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        players = new List<GameObject>();
        ActionCamera.instance.targets = new List<Transform>();
        foreach (PlayerProperties props in PlayerManager.players)
        {
            //add random spawns support
            GameObject newPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            newPlayer.GetComponent<PlayerInputHandler>().SetupInput(props.Device);
            players.Add(newPlayer);

            ActionCamera.instance.targets.Add(newPlayer.transform);
        }

    }
}

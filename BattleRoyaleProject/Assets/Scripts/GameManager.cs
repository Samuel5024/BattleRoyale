using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabLocation;
    public PlayerController[] players;
    public Transform[] spawnPoints;
    public int alivePlayers;

    private int playersInGame;

    // instance 
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // set the size of the players array and alive players interger
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        alivePlayers = players.Length;

        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    // ImInGame gets called to all players when someone loads into the Game scene
    // This keeps track of how many players are in the game and if everyone is in, spawn the players

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        
        if(PhotonNetwork.IsMasterClient && playersInGame == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("SpawnPlayer", RpcTarget.All);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Debug Option")]
    [Tooltip("Debug Massage On/Off Checkbox")]
    public bool debug = true;
    public GameObject severConnectBtn;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        if(debug)
            print("Connected Sever");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (debug)
            print("Join The Lobby");

        severConnectBtn.SetActive(true);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("MyRoom", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        if (debug)
            print("Room In");

        PhotonNetwork.LoadLevel("LevelOne");
    }
}

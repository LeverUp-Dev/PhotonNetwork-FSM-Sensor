using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Debug Option")]
    [Tooltip("Debug Massage On/Off Checkbox")]
    public bool debug = true;
    public GameObject severConnectBtn;

    string gameVersion = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Ŭ���̾�Ʈ���� �� ����ȭ ����
        PhotonNetwork.GameVersion = gameVersion;
        Screen.SetResolution(1920, 1080, false); //�ػ�

        PhotonNetwork.ConnectUsingSettings(); //�������� �� ����
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

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("LevelOne");
        }
    }
}

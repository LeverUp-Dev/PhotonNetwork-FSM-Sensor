using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
        Connect();
        JoinLobby();
    }

    private void Update()
    {
        //Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(); //서버에 접속    

    }
    public override void OnConnectedToMaster() //콜백 ConnectUsingSettings()
    {
        print("Connect!");
        PhotonNetwork.LocalPlayer.NickName = "Abback";
    }

    public void Disconnect() => PhotonNetwork.Disconnect(); //서버에 접속 실패
    public override void OnDisconnected(DisconnectCause cause) // 콜백 Disconnect()
    {        
        PhotonNetwork.ConnectUsingSettings(); // 재접속 시도
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(); //로비입장
    }
    public override void OnJoinedLobby() // 콜백 JoinLobby()
    {
        Debug.Log("Entered Lobby!");
        if(PhotonNetwork.IsConnected) // 재차확인
        {
            PhotonNetwork.JoinRandomRoom();
        }        
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom("MyRobby"); //룸생성

    public void JoinRoom() => PhotonNetwork.JoinRoom("MyRoom"); // 룸입장
    public override void OnJoinedRoom() // 콜백 JoinRoom()
    {
        Debug.Log("Enter Room");
        PhotonNetwork.LoadLevel("Scene Name"); //씬 로드
        CreatePlayer();
        if(PhotonNetwork.IsMasterClient)
        {
            // 방장만 할일
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // 빈 방이 없을때 주로 발생하므로 룸생성
    }

    void CreatePlayer() // Resources 폴더에 있는 플러이어 생성
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
    }



    public void JoinOrCreateRoom() => PhotonNetwork.JoinRandomOrCreateRoom();
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnLeftRoom()
    {
        Debug.Log("Left"); // 내가 나갈때만 실행
        //SceneManager.LoadScene("Lobby");
    }

}

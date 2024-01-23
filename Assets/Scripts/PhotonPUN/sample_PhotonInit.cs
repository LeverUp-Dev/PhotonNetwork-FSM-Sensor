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
        PhotonNetwork.ConnectUsingSettings(); //������ ����    

    }
    public override void OnConnectedToMaster() //�ݹ� ConnectUsingSettings()
    {
        print("Connect!");
        PhotonNetwork.LocalPlayer.NickName = "Abback";
    }

    public void Disconnect() => PhotonNetwork.Disconnect(); //������ ���� ����
    public override void OnDisconnected(DisconnectCause cause) // �ݹ� Disconnect()
    {        
        PhotonNetwork.ConnectUsingSettings(); // ������ �õ�
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(); //�κ�����
    }
    public override void OnJoinedLobby() // �ݹ� JoinLobby()
    {
        Debug.Log("Entered Lobby!");
        if(PhotonNetwork.IsConnected) // ����Ȯ��
        {
            PhotonNetwork.JoinRandomRoom();
        }        
    }

    public void CreateRoom() => PhotonNetwork.CreateRoom("MyRobby"); //�����

    public void JoinRoom() => PhotonNetwork.JoinRoom("MyRoom"); // ������
    public override void OnJoinedRoom() // �ݹ� JoinRoom()
    {
        Debug.Log("Enter Room");
        PhotonNetwork.LoadLevel("Scene Name"); //�� �ε�
        CreatePlayer();
        if(PhotonNetwork.IsMasterClient)
        {
            // ���常 ����
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // �� ���� ������ �ַ� �߻��ϹǷ� �����
    }

    void CreatePlayer() // Resources ������ �ִ� �÷��̾� ����
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
    }



    public void JoinOrCreateRoom() => PhotonNetwork.JoinRandomOrCreateRoom();
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnLeftRoom()
    {
        Debug.Log("Left"); // ���� �������� ����
        //SceneManager.LoadScene("Lobby");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using ExitGames.Client.Photon;

public class PlayerController : MonoBehaviour, IPunObservable, IOnEventCallback
{
    //레퍼런스
    public Material[] materials;
    PhotonView pv;
    // 위치 콜백
    Vector3 curPos;
    Quaternion curRot;
    float hp = 100;
    //포탄
    public GameObject bullet;
    public Transform firePos;
    //포탄 충돌 처리
    MeshRenderer[] renderers;
    public Image hpBar;
    public Canvas hudCanvas;
    public Text hpTxt;
    //이벤트 함수
    int tankColor = 0;
    public const byte SendColorEvent = 1;

    void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();

        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            GetComponent<Renderer>().material = materials[0];
        }
        else
        {
            GetComponent<Renderer>().material = materials[1];
        }
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(pv.IsMine)
        {
            transform.Translate(Vector3.forward * v * Time.deltaTime * 10);
            transform.Rotate(Vector3.up * h * Time.deltaTime * 100);

            if(Input.GetMouseButtonDown(0))
            {
                pv.RPC("RPCFire", RpcTarget.All);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, curRot, Time.deltaTime * 10);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(hp);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
            hp = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void RPCFire()
    {
        GameObject bulletObj = Instantiate(bullet, firePos.position, transform.rotation);
        Destroy(bulletObj, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);

            hp -= 20;
            hpTxt.text = hp.ToString();
            hpBar.fillAmount = hp * 0.01f;

            if(hp <= 0)
            {
                SendEvent(); // Raise Event
                StartCoroutine("ExplosionTank");
            }
        }
    }

    IEnumerator ExplosionTank()
    {
        TankVisible(false);

        yield return new WaitForSeconds(3);
        TankVisible(true);
        hp = 100;
        hpTxt.text = hp.ToString();
        hpBar.fillAmount = 1;
    }

    void TankVisible(bool v)
    {
        foreach(MeshRenderer meshRen in renderers)
        {
            meshRen.enabled = v;
        }

        hudCanvas.enabled = v;
    }

    void SendEvent()
    {
        object[] content = new object[] { new Vector3(0, 0, 0) };
        PhotonNetwork.RaiseEvent(SendColorEvent, content,
                                                        RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == SendColorEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            Vector3 temp = (Vector3)data[0];
            if(pv.IsMine)
            {
                GetComponent<Renderer>().material = materials[tankColor++ % 3];
            }
            else
            {
                print(temp);
                transform.position = temp;
            }
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}

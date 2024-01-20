using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour, IPunObservable
{
    //레퍼런스
    public Material[] materials;

    PhotonView pv;
    // 위치 콜백
    Vector3 curPos;
    Quaternion curRot;
    float hp = 100;
    //shoot
    public GameObject bullet;
    public Transform firePos;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
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

    void Start()
    {
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
            //transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10);
            //transform.rotation = Quaternion.Lerp(transform.rotation, currRot, Time.deltaTime * 10);
        }
    }

    [PunRPC]
    void RPCFire()
    {
        GameObject bulletObj = Instantiate(bullet, firePos.position, transform.rotation);
        Destroy(bulletObj, 5);
    }
}

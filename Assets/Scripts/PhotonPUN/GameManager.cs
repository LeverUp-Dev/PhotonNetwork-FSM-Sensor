using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{ 
    void Awake()
    {
        Vector3 startPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
        GameObject Player = PhotonNetwork.Instantiate("Player", startPosition, Quaternion.identity);
    }
}

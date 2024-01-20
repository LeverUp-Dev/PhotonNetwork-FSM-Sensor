using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    int dist = 5;
    int height = 3;

    public Transform player;

    Vector3 offset;

    void Update()
    {
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player").transform;

            offset = player.forward * dist + Vector3.down * height;
            transform.position = Vector3.Lerp(transform.position, player.position - offset, 0.05f);
            transform.LookAt(offset);
        }
    }
}

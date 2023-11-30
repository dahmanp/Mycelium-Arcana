using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomsTriggers: MonoBehaviourPun
{
    public GameObject[] spawns;
    public GameObject[] triggers;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject == triggers[0])
            {
                other.gameObject.transform.position = spawns[0].transform.position;
            }
            else if (this.gameObject == triggers[1])
            {
                other.gameObject.transform.position = spawns[1].transform.position;
            }
            else if (this.gameObject == triggers[2])
            {
                other.gameObject.transform.position = spawns[2].transform.position;
            }
        }
    }
}

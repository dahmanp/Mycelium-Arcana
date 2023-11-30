using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameStartTrigger : MonoBehaviourPun
{
    public GameObject[] spawns;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.name == "Holothurian(Clone)")
            {
                other.gameObject.transform.position = spawns[0].transform.position;
            }
            else if (other.gameObject.name == "Etori(Clone)")
            {
                other.gameObject.transform.position = spawns[1].transform.position;
            }
            else if (other.gameObject.name == "Velkivon(Clone)")
            {
                other.gameObject.transform.position = spawns[2].transform.position;
            }
            else if (other.gameObject.name == "Avem(Clone)")
            {
                other.gameObject.transform.position = spawns[3].transform.position;
            }
        }
    }
}

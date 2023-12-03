using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class KeyTrigger : MonoBehaviourPun
{
    public GameObject key;
    public GameObject trigger;
    public string season;

    [PunRPC]
    void Start()
    {
        SpriteRenderer k = key.GetComponent<SpriteRenderer>();
        k.enabled = false;
    }

    [PunRPC]
    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer k = key.GetComponent<SpriteRenderer>();
        if (other.gameObject.CompareTag(season))
        {
            if (other.gameObject.GetComponent<PlayerController>().instance.hasKey == true)
            {
                k.enabled = true;
                GameManager.instance.totalKeys++;
                other.gameObject.GetComponent<PlayerController>().instance.hasKey = false;
            }
        }
    }
}

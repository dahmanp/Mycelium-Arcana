using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum PickupType
{
    Magic,
    Health,
    Key
}

public class Pickup : MonoBehaviourPun
{
    public PickupType type;
    public int value;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (type == PickupType.Key)
        {
            player.photonView.RPC("GiveKey", player.photonPlayer);
        }
        else if (type == PickupType.Health)
        {
            player.photonView.RPC("Heal", player.photonPlayer, value);
        }
        else if (type == PickupType.Magic)
        {
            player.photonView.RPC("Magic", player.photonPlayer, value);
        }
        PhotonNetwork.Destroy(gameObject);
        /*
        if (other.gameObject.CompareTag("Holothurian") || other.gameObject.CompareTag("Etori") || other.gameObject.CompareTag("Velkivon") || other.gameObject.CompareTag("Avem"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (type == PickupType.Key)
            {
                player.photonView.RPC("GiveKey", player.photonPlayer);
            }
            else if (type == PickupType.Health)
            {
                player.photonView.RPC("Heal", player.photonPlayer, value);
            }
            else if (type == PickupType.Magic)
            {
                player.photonView.RPC("Magic", player.photonPlayer, value);
            }
            PhotonNetwork.Destroy(gameObject); 
        } */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NPCDialogue : MonoBehaviourPun
{
    public GameObject trigger;
    public Button yesButton;
    public GameObject bg;
    public GameObject nametag;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;

    public int clicks;
    public int players;
    public GameObject jade;

    [PunRPC]
    public void YESButton()
    {
        yesButton.interactable = false;
        clicks++;
    }

    [PunRPC]
    void OnTriggerEnter2D(Collider2D other)
    {
        players = PhotonNetwork.PlayerList.Length;
        if (other.gameObject.CompareTag("Holothurian"))
        {
            nametag.SetActive(true);
            bg.SetActive(true);
            text1.SetActive(true);
            Invoke("deactivate1", 2.0f);
            Invoke("activate2", 2.0f);
            Invoke("deactivate2", 2.0f);
            Invoke("activate3", 2.0f);
            if (clicks == players)
            {
                Debug.Log("urmom");
                Invoke("deactivate3", 2.0f);
                Invoke("activate4", 2.0f);
                Invoke("deactivate4", 2.0f);
                Invoke("deactivatebg", 2.0f);
                Invoke("deactivatetag", 2.0f);
                trigger.GetComponent<BoxCollider2D>().enabled = false;
                //jade.Play();
            }
        }
    }

    void activate2()
    {
        text2.SetActive(true);
    }

    void activate3()
    {
        text3.SetActive(true);
    }

    void activate4()
    {
        text4.SetActive(true);
    }

    void deactivate1()
    {
        text1.SetActive(false);
    }

    void deactivate2()
    {
        text2.SetActive(false);
    }

    void deactivate3()
    {
        text3.SetActive(false);
    }

    void deactivate4()
    {
        text4.SetActive(false);
    }

    void deactivatebg()
    {
        bg.SetActive(false);
    }

    void deactivatetag()
    {
        nametag.SetActive(false);
    }
}

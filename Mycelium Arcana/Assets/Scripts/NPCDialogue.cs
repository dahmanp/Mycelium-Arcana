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
    public GameObject temp;
    public GameObject bg;
    public GameObject nametag;
    public TextMeshProUGUI textObj;
    public GameObject jadeText;

    public int clicks;
    public int players;
    public GameObject jade;

    [PunRPC]
    public void YESButton()
    {
        if (photonView.IsMine)
        {
            yesButton.interactable = false;
            photonView.RPC("AddClicks", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void AddClicks()
    {
        clicks++;
        if (clicks == players)
        {
            photonView.RPC("FinishDialogue", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void Start()
    {
        textObj.GetComponent<TextMeshProUGUI>().text = "";
        players = PhotonNetwork.PlayerList.Length;
    }


    [PunRPC]
    void OnTriggerEnter2D(Collider2D other)
    {
        trigger.GetComponent<BoxCollider2D>().enabled = false;
        if (other.gameObject.CompareTag("Holothurian"))
        {
            nametag.SetActive(true);
            bg.SetActive(true);
            StartCoroutine(DialogueStart());
        }
    }

    void setText(string message)
    {
        textObj.text = message;
    }

    IEnumerator DialogueStart()
    {
        setText("I'm glad you all are here!");
        yield return new WaitForSeconds(2.0f);

        setText("Please, purge the decay that plagues\n" + "our temple...");
        yield return new WaitForSeconds(2.0f);

        setText("Will you accept my quest?");
        temp.SetActive(true);
    }

    [PunRPC]
    void FinishDialogue()
    {
        setText("Thank you, and please be careful...");
        temp.SetActive(false);
        Invoke("deactivation", 2.0f);
    }

    void deactivation()
    {
        jadeText.SetActive(false);
    }
}
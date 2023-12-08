using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string summer;
    public string fall;
    public string winter;
    public string spring;

    [Header("Misc")]
    public GameObject fallKey;
    public GameObject winterKey;
    public GameObject springKey;

    public int keyAmt;

    public Transform[] spawnPoints;
    public float respawnTime;
    private int playersInGame;
    public PlayerController[] players;

    public GameObject boss;
    public GameObject winBackground;
    public GameObject winBackgroundBlank;
    public GameObject winColor;
    public int totalKeys;
    public GameObject bossBlock;
    public bool bossDied = false;

    public int clicks;
    public Button yesButton;

    // instance
    public static GameManager instance;
    void Awake()
    {
        boss.SetActive(false);
        instance = this;
    }

    [PunRPC]
    void setKeys()
    {
        SpriteRenderer f = fallKey.GetComponent<SpriteRenderer>();
        SpriteRenderer w = winterKey.GetComponent<SpriteRenderer>();
        SpriteRenderer s = springKey.GetComponent<SpriteRenderer>();

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            totalKeys = 3;
            f.enabled = true;
            w.enabled = true;
            s.enabled = true;
        }
        else if (PhotonNetwork.PlayerList.Length == 2)
        {
            totalKeys = 2;
            w.enabled = true;
            s.enabled = true;
        }
        else if (PhotonNetwork.PlayerList.Length == 3)
        {
            totalKeys = 1;
            s.enabled = true;
        }
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
        Invoke("setKeys", 5.0f);
    }

    void SpawnPlayer()
    {
        GameObject playerObj;
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            playerObj = PhotonNetwork.Instantiate(summer, spawnPoints[0].position, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            playerObj = PhotonNetwork.Instantiate(fall, spawnPoints[1].position, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            playerObj = PhotonNetwork.Instantiate(winter, spawnPoints[2].position, Quaternion.identity);
        }
        else
        {
            playerObj = PhotonNetwork.Instantiate(spring, spawnPoints[3].position, Quaternion.identity);
        }

        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);

    }

    [PunRPC]
    void Update()
    {
        if (totalKeys == 4)
        {
            boss.SetActive(true);
            bossBlock.SetActive(false);
        }
    }

    public void SetWinText()
    {
        winBackground.SetActive(true);
        winColor.SetActive(true);
    }

    [PunRPC]
    public void WinGame()
    {
        if (bossDied == true)
        {
            bossDied = false;
            Debug.Log("Works");
            winBackgroundBlank.SetActive(true);
            Invoke("SetWinText", 1.0f);
            //Invoke("GoBackToMenu", 5.0f);
        }
    }

    [PunRPC]
    public void MenuButtonTime()
    {
        yesButton.interactable = false;
        photonView.RPC("AddClicks", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void AddClicks()
    {
        clicks++;
        if (clicks == playersInGame)
        {
            PhotonNetwork.LoadLevel("Menu");
        }
    }
}

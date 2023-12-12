using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject createRoomScreen;
    public GameObject lobbyScreen;
    public GameObject lobbyBrowserScreen;
    public GameObject startScreen;
    public GameObject creditsScreen;
    public GameObject settingsScreen;
    public GameObject leaderboard1;
    public GameObject leaderboard2;
    public GameObject leaderboard3;
    public GameObject bg;

    [Header("Main Screen")]
    public Button createRoomButton;
    public Button findRoomButton;

    [Header("Lobby")]
    public TextMeshProUGUI[] playerText;
    public TextMeshProUGUI roomInfoText;
    public Button startGameButton;

    [Header("Lobby Browser")]
    public RectTransform roomListContainer;
    public GameObject roomButtonPrefab;

    private int partyNum;
    private List<GameObject> roomButtons = new List<GameObject>();
    private List<RoomInfo> roomList = new List<RoomInfo>();

    void Start()
    {
        leaderboard2.SetActive(true);
        leaderboard3.SetActive(true);
        createRoomButton.interactable = false;
        findRoomButton.interactable = false;
        // enable the cursor since we hide it when we play the game
        Cursor.lockState = CursorLockMode.None;
        // are we in a game?
        if (PhotonNetwork.InRoom)
        {

            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    void SetScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        lobbyBrowserScreen.SetActive(false);
        startScreen.SetActive(false);
        creditsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        leaderboard1.SetActive(false);
        leaderboard2.SetActive(false);
        leaderboard3.SetActive(false);

        screen.SetActive(true);

        if (screen == lobbyBrowserScreen)
            UpdateLobbyBrowserUI();
    }

    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        findRoomButton.interactable = true;
    }

    public void OnCreateRoomButton()
    {
        bg.SetActive(true);
        SetScreen(createRoomScreen);
    }

    public void OnFindRoomButton()
    {
        bg.SetActive(true);
        SetScreen(lobbyBrowserScreen);
    }

    public void ToMainScreen()
    {
        bg.SetActive(true);
        SetScreen(mainScreen);
    }

    public void OnCreditsButton()
    {
        bg.SetActive(true);
        SetScreen(creditsScreen);
    }

    public void OnLeaderboardsButton()
    {
        SetScreen(leaderboard1);
        Invoke("activation", 0.0001f);
        bg.SetActive(false);
    }

    void activation()
    {
        leaderboard2.SetActive(true);
        leaderboard3.SetActive(true);
    }

    public void OnCreateButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnSettingsButton()
    {
        bg.SetActive(true);
        SetScreen(settingsScreen);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.NickName = PlayerInfo.instance.profile.DisplayName;
        SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    [PunRPC]
    void UpdateLobbyUI()
    {
        startGameButton.interactable = PhotonNetwork.IsMasterClient;

        int i = 0;

        // display all the players
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            playerText[i].text = player.NickName;
            partyNum++;
            i++;
        }

        // set the room info text
        roomInfoText.text = "<b>Party Name: </b>" + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    public void OnStartGameButton()
    {
        // hide the room
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        // tell everyone to load the game scene
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    GameObject CreateRoomButton()
    {
        GameObject buttonObj = Instantiate(roomButtonPrefab, roomListContainer.transform);
        roomButtons.Add(buttonObj);
        return buttonObj;
    }

    public void OnJoinRoomButton(string roomName)
    {
        NetworkManager.instance.JoinRoom(roomName);
    }

    public void OnRefreshButton()
    {
        UpdateLobbyBrowserUI();
    }

    public override void OnRoomListUpdate(List<RoomInfo> allRooms)
    {
        roomList = allRooms;
    }

    void UpdateLobbyBrowserUI()
    {
        // disable all room buttons
        foreach (GameObject button in roomButtons)
            button.SetActive(false);
        // display all current rooms in the master server
        for (int x = 0; x < roomList.Count; ++x)
        {
            // get or create the button object
            GameObject button = x >= roomButtons.Count ? CreateRoomButton() : roomButtons
           [x];
            button.SetActive(true);
            // set the room name and player count texts
            button.transform.Find("RoomNameText").GetComponent<TextMeshProUGUI>().text = roomList[x].Name;
            button.transform.Find("PlayerCountText").GetComponent<TextMeshProUGUI>().text = partyNum + " / 4";

            // set the button OnClick event
            Button buttonComp = button.GetComponent<Button>();
            string roomName = roomList[x].Name;
            buttonComp.onClick.RemoveAllListeners();
            buttonComp.onClick.AddListener(() => { OnJoinRoomButton(roomName); });
        }
    }
}

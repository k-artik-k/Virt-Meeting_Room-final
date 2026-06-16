using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Input Fields")]
    public TMP_InputField playerNameField;
    public TMP_InputField roomCodeField;
    public TMP_InputField passwordField;

    [Header("Buttons")]
    public Button createButton;
    public Button joinButton;

    [Header("Scene")]
    public string mainSceneName = "Main";

    [Header("UI Feedback")]
    public TMP_Text statusText;

    private const string PASSWORD_PROP_KEY = "pwd";
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    void Start()
    {
        createButton.onClick.AddListener(OnCreateClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
        SetButtons(false);
        SetStatus("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        SetStatus("Connected.");
    }

    public override void OnJoinedLobby() => SetButtons(true);

   public override void OnCreatedRoom()
{
    SetStatus("Room created!");
    GameSession.RoomCode = roomCodeField.text.Trim();
    GameSession.PlayerName = playerNameField.text.Trim();
    GameSession.Password = passwordField.text.Trim();
    SceneManager.LoadScene(mainSceneName);
}

public override void OnJoinedRoom()
{
    SetStatus("Joined!");
    GameSession.RoomCode = roomCodeField.text.Trim();
    GameSession.PlayerName = playerNameField.text.Trim();
    SceneManager.LoadScene(mainSceneName);
}

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetStatus($"Create failed: {message}");
        SetButtons(true);
    }

    // public override void OnJoinedRoom()
    // {
    //     SetStatus("Joined!");
    //     SceneManager.LoadScene(mainSceneName);
    // }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetStatus($"Join failed: {message}");
        SetButtons(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("DISCONNECT REASON: " + cause);
        SetStatus("Disconnected: " + cause);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList.Clear();
        foreach (var r in roomList)
            if (!r.RemovedFromList) cachedRoomList.Add(r);
    }

    void OnCreateClicked()
    {
        if (!ValidateInputs()) return;
        SetButtons(false);
        PhotonNetwork.NickName = playerNameField.text.Trim();

        var options = new RoomOptions
        {
            MaxPlayers = 10,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {{ PASSWORD_PROP_KEY, passwordField.text.Trim() }},
            CustomRoomPropertiesForLobby = new string[] { PASSWORD_PROP_KEY }
        };
        PhotonNetwork.CreateRoom(roomCodeField.text.Trim(), options);
    }

    void OnJoinClicked()
    {
        if (!ValidateInputs()) return;
        SetButtons(false);
        PhotonNetwork.NickName = playerNameField.text.Trim();
        StartCoroutine(JoinWithPasswordCheck(roomCodeField.text.Trim(), passwordField.text.Trim()));
    }

    IEnumerator JoinWithPasswordCheck(string roomCode, string entered)
    {
        yield return new WaitForSeconds(0.3f);
        var target = cachedRoomList.Find(r => r.Name == roomCode);

        if (target != null && target.CustomProperties.ContainsKey(PASSWORD_PROP_KEY))
        {
            string stored = target.CustomProperties[PASSWORD_PROP_KEY].ToString();
            if (!string.IsNullOrEmpty(stored) && stored != entered)
            {
                SetStatus("Wrong password.");
                SetButtons(true);
                yield break;
            }
        }
        PhotonNetwork.JoinRoom(roomCode);
    }

    bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(playerNameField.text)) { SetStatus("Enter your name."); return false; }
        if (string.IsNullOrWhiteSpace(roomCodeField.text)) { SetStatus("Enter room code."); return false; }
        return true;
    }

    void SetButtons(bool state) { createButton.interactable = state; joinButton.interactable = state; }
    void SetStatus(string msg) { if (statusText) statusText.text = msg; Debug.Log("[RoomManager] " + msg); }
}
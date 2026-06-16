using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [Header("Spawn Settings")]
    public string playerPrefabName = "model";
    public Transform[] spawnPoints;

    private GameObject spawnedPlayer;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
        else
        {
            PhotonNetwork.NickName = GameSession.PlayerName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(
            GameSession.RoomCode,
            new RoomOptions { MaxPlayers = 10 },
            null
        );
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (spawnedPlayer != null) return;

        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int index = PhotonNetwork.LocalPlayer.ActorNumber % spawnPoints.Length;
            spawnPos = spawnPoints[index].position;
            spawnRot = spawnPoints[index].rotation;
        }

        spawnedPlayer = PhotonNetwork.Instantiate(
            playerPrefabName,
            spawnPos,
            spawnRot
        );
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[PlayerSpawner] Disconnected: " + cause);
    }
}
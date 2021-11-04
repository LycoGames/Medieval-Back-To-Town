using UnityEngine;
using Photon.Pun;
using System;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int multiplayerSceneIndex;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Starting Game");
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }
}

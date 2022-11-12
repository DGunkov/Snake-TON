using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _roomName;
    private int _skinIndex;
    public List<GameObject> Skins;

    private void Awake()
    {
        _roomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.JoinLobby();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void Previous()
    {

    }

    public void Next()
    {

    }
}

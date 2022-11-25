using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private Text _roomName;
    private LobbyManager _manager;
    public RoomInfo roomInfo { get; private set; }

    private void Start()
    {
        _manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomInfo(RoomInfo info)
    {
        roomInfo = info;
        _roomName.text = roomInfo.Name;
    }

    public void JoinRoom()
    {
        DataHolder.RoomName = _roomName.text;
        // SceneManager.LoadScene("Room");

        _manager.SwitchToRoom();
    }
}

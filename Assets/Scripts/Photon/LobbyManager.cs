using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _createInput;
    [SerializeField] private InputField _joinInput;
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private Text _roomName;
    [SerializeField] private RoomItem _roomItem;
    [SerializeField] private Transform _contentObject;
    [SerializeField] private float _timeBetweenUpdates = 1.5f;

    private float _nextUpdateTime;
    private List<RoomItem> _roomItems = new List<RoomItem>();

    public void CreateRoom()
    {
        if (_createInput.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(_createInput.text, new RoomOptions() { MaxPlayers = 10 });
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_joinInput.text);
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        // _lobbyPanel.SetActive(false);
        // _roomPanel.SetActive(true);
        // _roomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= _nextUpdateTime)
        {
            UpdateRoomList(roomList);
            _nextUpdateTime = Time.time + _timeBetweenUpdates;
        }
    }

    private void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in _roomItems)
        {
            Destroy(item.gameObject);
        }
        _roomItems.Clear();

        foreach (RoomInfo room in list)
        {
            if (room.RemovedFromList)
            {
                int index = _roomItems.FindIndex(x => x.roomInfo.Name == room.Name);
                if (index != -1)
                {
                    Destroy(_roomItems[index].gameObject);
                    _roomItems.RemoveAt(index);
                }
            }
            else
            {
                RoomItem newRoom = Instantiate(_roomItem, _contentObject);
                if (newRoom != null)
                {
                    newRoom.SetRoomInfo(room);
                    _roomItems.Add(newRoom);
                }
            }
        }
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        _roomPanel.SetActive(false);
        _lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}

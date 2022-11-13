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

    private void Start()
    {
        // PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
        // _nextUpdateTime = 0f;
    }

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
        PhotonNetwork.LoadLevel("Room");
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
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}

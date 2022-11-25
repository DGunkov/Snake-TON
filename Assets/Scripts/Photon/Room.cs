using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _roomName;
    [SerializeField] private InputField _crystallsInput;
    [SerializeField] private Toggle _mouseInput;
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private LobbyManager _lobbyManager;
    private int _skinIndex = 0;
    public List<GameObject> Skins;

    public override void OnEnable()
    {
        base.OnEnable();
        _roomName.text = "Room: " + DataHolder.RoomName;
        Skins[_skinIndex].SetActive(true);
    }

    public void LeaveRoom()
    {
        _lobbyManager.SwitchToLobby();
    }

    public void StartGame()
    {
        DataHolder.SkinIndex = _skinIndex;
        DataHolder.MouseInput = _mouseInput.isOn;
        DataHolder.CrystallsEntered = int.Parse(_crystallsInput.text);
        if (_lobbyManager.Creating)
        {
            PhotonNetwork.CreateRoom(DataHolder.RoomName);
        }
        else
        {
            PhotonNetwork.JoinRoom(DataHolder.RoomName);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void Previous()
    {
        if (_skinIndex - 1 >= 0)
        {
            int previousIndex = _skinIndex;
            _skinIndex--;
            Skins[previousIndex].SetActive(false);
            Skins[_skinIndex].SetActive(true);
        }
    }

    public void Next()
    {
        if (_skinIndex + 1 <= Skins.Count - 1)
        {
            int previousIndex = _skinIndex;
            _skinIndex++;
            Skins[previousIndex].SetActive(false);
            Skins[_skinIndex].SetActive(true);
        }
    }
}

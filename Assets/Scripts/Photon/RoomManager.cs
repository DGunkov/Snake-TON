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
    [SerializeField] private InputField _crystallsInput;
    [SerializeField] private Toggle _mouseInput;
    private int _skinIndex = 0;
    public List<GameObject> Skins;

    private void Awake()
    {
        _roomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
        Skins[_skinIndex].SetActive(true);
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
        DataHolder.SkinIndex = _skinIndex;
        DataHolder.MouseInput = _mouseInput.isOn;
        DataHolder.CrystallsEntered = int.Parse(_crystallsInput.text);
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

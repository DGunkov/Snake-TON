using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class UiManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Slider _stamina;
    [SerializeField] private Text _speedValue;
    [SerializeField] private List<Player> _players = new List<Player>();
    [SerializeField] GameObject _playerItem;
    [SerializeField] Transform _content;

    private GameObject _player;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerInput>().gameObject;
        _stamina.maxValue = _player.GetComponent<Movement>().MaxEnergy;
        GetComponent<Canvas>().worldCamera = GetComponentInParent<PlayerInput>().gameObject.GetComponentInChildren<Camera>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            _players.Add(player);
        }
    }

    private void Update()
    {
        _stamina.value = _player.GetComponent<Movement>().Energy;
        _speedValue.text = (_player.GetComponent<Movement>().Speed * 10).ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _players.Add(newPlayer);
        UpdateUI(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _players.Remove(otherPlayer);
        UpdateUI(otherPlayer);
    }
    private void UpdateUI(Player player)
    {
        GameObject playerItem = GameObject.Instantiate(_playerItem, _content);
        playerItem.GetComponentInChildren<Text>().text = player.NickName;
    }
}

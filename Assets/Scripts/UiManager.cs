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
    [SerializeField] private List<GameObject> _playerItems = new List<GameObject>();
    [SerializeField] GameObject _playerItem;
    [SerializeField] Transform _content;

    private GameObject _player;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerInput>().gameObject;
        _stamina.maxValue = _player.GetComponent<Movement>().MaxEnergy;
        _stamina.value = _stamina.maxValue;
        GetComponent<Canvas>().worldCamera = GetComponentInParent<PlayerInput>().gameObject.GetComponentInChildren<Camera>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddPlayer(player);
        }
    }

    private void Update()
    {
        _stamina.value = _player.GetComponent<Movement>().Energy;
        _speedValue.text = Mathf.RoundToInt((_player.GetComponent<Movement>().Speed * 10)).ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayer(newPlayer);
    }

    private void AddPlayer(Player player)
    {
        _players.Add(player);
        GameObject playerItem = GameObject.Instantiate(_playerItem, _content);
        playerItem.GetComponentInChildren<Text>().text = player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _players.Remove(otherPlayer);
        foreach (GameObject playerItem in _playerItems)
        {
            if (playerItem.GetComponentInChildren<Text>().text == otherPlayer.NickName)
            {
                Destroy(playerItem);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class UiManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Slider _stamina;
    [SerializeField] private Text _speedValue;
    [SerializeField] private List<Player> _players = new List<Player>();
    [SerializeField] private List<GameObject> _playerItems = new List<GameObject>();
    [SerializeField] GameObject _playerItem;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _emojy;
    [SerializeField] GameObject _death_panel;
    [SerializeField] GameObject _stats_panel;
    [SerializeField] TMP_Text _kills;
    [SerializeField] TMP_Text _collect;


    private GameObject _player;

    private bool pause_game;
    private void Awake()
    {
        _stats_panel.SetActive(false);
        _death_panel.SetActive(false);
        if (!transform.parent.gameObject.GetComponent<PhotonView>().IsMine)
        {
            gameObject.SetActive(false);
        }
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


    public void SelectEmojy(int i)
    {
        _emojy.GetComponent<Emojy>()._time_to_off = 1;
        _emojy.SetActive(true);
    }
    public void AutoPilot()
    {
        GameObject parent = transform.parent.gameObject;
        if(parent.GetComponent<NPC>().enabled == false)
        {
            parent.GetComponent<PlayerInput>().auto_pilot = true;
            parent.GetComponent<NPC>().enabled = true;
        }
        else
        {
            parent.GetComponent<PlayerInput>().auto_pilot = false;
            parent.GetComponent<NPC>().enabled = false;
        }
    }
    public void Pause()
    {
        if(pause_game)
        {
            Time.timeScale = 1;
            pause_game = false;
        }
        else
        {
            Time.timeScale = 0;
            pause_game = true;
        }
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

    internal void SwitchDeatchPanel(bool stay)
    {
        _death_panel.SetActive(stay);
    }

    public void Stats()
    {
        _death_panel.SetActive(false);
        _stats_panel.SetActive(true);
        int k = transform.parent.GetComponent<PlayerInput>()._kills;
        int c = transform.parent.GetComponent<PlayerInput>()._collect_crystalls;
        _collect.text = "Kills: " + k.ToString();
        _kills.text = "Crystalls: " + c.ToString();
    }
}

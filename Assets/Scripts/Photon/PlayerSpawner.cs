using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _skins;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private GameObject _NPCPrefab;

    [SerializeField] private float _minimalX;
    [SerializeField] private float _maximumX;
    [SerializeField] private float _minimalY;
    [SerializeField] private float _maximumY;

    private void Start()
    {
        _playerPrefab = _skins[DataHolder.SkinIndex];
        Vector2 randomPosition = new Vector2(Random.Range(_minimalX, _maximumX), Random.Range(_minimalY, _maximumY));
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, randomPosition, Quaternion.identity);
        GameObject camera = PhotonNetwork.Instantiate(_cameraPrefab.name, player.transform.position, Quaternion.identity);
        SpawnNPC(randomPosition);
        player.GetComponent<PlayerInput>().Camera = camera;
        // player.GetComponentInChildren<Canvas>().worldCamera = camera.GetComponent<Camera>();
        player.GetComponent<PlayerInput>().CrystallsEntered = DataHolder.CrystallsEntered;
        camera.GetComponent<CameraMovement>().Snake = player.transform;
    }

    private void SpawnNPC(Vector2 randomPosition)
    {
        Vector2 randomPos = new Vector2(Random.Range(_minimalX, _maximumX), Random.Range(_minimalY, _maximumY));
        GameObject NPC = PhotonNetwork.InstantiateRoomObject(_NPCPrefab.name, randomPos, Quaternion.identity);
        for (int i = 0; i < DataHolder.CrystallsEntered / 2; i++)
        {
            NPC.GetComponent<Mass>().AddMass(1);
        }
    }
}

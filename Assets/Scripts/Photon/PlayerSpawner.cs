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
    [SerializeField] private int NPCCount = 2;

    private void Start()
    {
        _playerPrefab = _skins[DataHolder.SkinIndex];
        Vector2 randomPosition = new Vector2(Random.Range(_minimalX, _maximumX), Random.Range(_minimalY, _maximumY));
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, randomPosition, Quaternion.identity);
        GameObject camera = PhotonNetwork.Instantiate(_cameraPrefab.name, player.transform.position, Quaternion.identity);
        SpawnNPC();
        player.GetComponent<PlayerInput>().Camera = camera;
        player.GetComponent<PlayerInput>().CrystallsEntered = DataHolder.CrystallsEntered;
        camera.GetComponent<CameraMovement>().Snake = player.transform;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    private void SpawnNPC()
    {
        for (int i = 0; i < NPCCount; i++)
        {

            Vector2 randomPos = new Vector2(Random.Range(_minimalX, _maximumX), Random.Range(_minimalY, _maximumY));
            GameObject NPC = PhotonNetwork.InstantiateRoomObject(_NPCPrefab.name, randomPos, Quaternion.identity);
            for (int j = 0; j < DataHolder.CrystallsEntered / 2 / NPCCount; j++)
            {
                NPC.GetComponent<Mass>().AddMass(1);
            }
        }
    }
}

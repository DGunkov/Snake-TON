using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> _skins;

    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private GameObject _NPCPrefab;

    [SerializeField] private float _minimalX;
    [SerializeField] private float _maximumX;
    [SerializeField] private float _minimalY;
    [SerializeField] private float _maximumY;
    [SerializeField] private int NPCCount = 2;

    private GameObject _uiCamera;

    private void Awake()
    {
        Vector2 randomPosition = new Vector2(Random.Range(_minimalX, _maximumX), Random.Range(_minimalY, _maximumY));

        GameObject player = PhotonNetwork.Instantiate(_skins[DataHolder.SkinIndex].name, randomPosition, Quaternion.identity);
        GameObject camera = PhotonNetwork.Instantiate(_cameraPrefab.name, player.transform.position, Quaternion.identity);

        SpawnNPC();

        player.GetComponent<PlayerInput>().Camera = camera;
        _uiCamera = player.GetComponentInChildren<Camera>().gameObject;
        player.GetComponentInChildren<Canvas>().worldCamera = _uiCamera.GetComponent<Camera>();

        player.GetComponent<PlayerInput>().CrystallsEntered = DataHolder.CrystallsEntered;

        camera.GetComponent<CameraMovement>().Snake = player.transform;
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

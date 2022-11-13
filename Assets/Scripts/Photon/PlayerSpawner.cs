using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _skins;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _cameraPrefab;

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
        player.GetComponent<PlayerInput>().Camera = camera;
        camera.GetComponent<CameraMovement>().Snake = player.transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[RequireComponent(typeof(Mass))]
[RequireComponent(typeof(Movement))]
public class Grow : MonoBehaviour
{
    private PlayerInput _input;
    private Movement _movement;
    private Mass _mass;
    private FoodManager _foodManager;

    [SerializeField] private GameObject _tail;

    [SerializeField] private GameObject _bodyPart;
    [SerializeField] private float _partOffset = 0.1f;
    [SerializeField] private float _minimalSpeed = 1.2f;
    [SerializeField] private float _speedMultyplier = 0.98f;
    [SerializeField] private float _sizeMultyplier = 1.005f;
    [SerializeField] private float _minimalRotationSpeed = 150f;
    [SerializeField] private float _rotationSpeedMultyplier = 0.99f;

    public List<GameObject> Parts = new List<GameObject>();
    private List<GameObject> _bodyParts = new List<GameObject>();

    private void OnEnable()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            _mass.OnMassFilled += GrowUp;
            _mass.OnMassDeFilled += DeletePart;
            _movement.OnDeath += Death;
        }
    }

    private void OnDisable()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            _mass.OnMassFilled -= GrowUp;
            _mass.OnMassDeFilled -= DeletePart;
            _movement.OnDeath -= Death;
        }
    }

    private void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            Parts.Add(this.gameObject);
            _tail = PhotonNetwork.Instantiate(_tail.name, this.transform.position, this.transform.rotation);
            _tail.GetComponent<Tail>().Parent = Parts[Parts.Count - 1];
            Parts.Add(_tail);
            _mass = GetComponent<Mass>();
            _movement = GetComponent<Movement>();
            _foodManager = FindObjectOfType<FoodManager>();
            if (GetComponent<PlayerInput>() != null)
            {
                _input = GetComponent<PlayerInput>();
            }
        }
    }

    private void GrowUp()
    {
        GameObject parent = Parts[Parts.Count - 2];
        Vector3 parentPosition = parent.transform.position;
        parentPosition.x -= _partOffset;
        GameObject bodyPart = PhotonNetwork.Instantiate(_bodyPart.name, parentPosition, parent.transform.rotation) as GameObject;
        _bodyParts.Add(bodyPart);
        bodyPart.GetComponentInChildren<SpriteRenderer>().sortingOrder = -(_bodyParts.IndexOf(bodyPart));
        _tail.GetComponent<Tail>().Parent = bodyPart;
        _tail.GetComponentInChildren<SpriteRenderer>().sortingOrder = bodyPart.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;

        BodyPart partScript = bodyPart.GetComponent<BodyPart>();
        Movement movement = GetComponent<Movement>();

        partScript.ParentMovement = movement;
        partScript.Parent = parent;
        Parts.Remove(_tail);
        Parts.Add(bodyPart);
        Parts.Add(_tail);

        AdjustSpeed(movement);
        AdjustRotationSpeed(movement);

        transform.localScale *= _sizeMultyplier;
    }

    private void AdjustRotationSpeed(Movement movement)
    {
        if (movement.RotationSpeed > _minimalRotationSpeed)
        {
            movement.RotationSpeed *= _rotationSpeedMultyplier;
        }
        else if (movement.RotationSpeed < _minimalRotationSpeed)
        {
            movement.RotationSpeed = _minimalRotationSpeed;
        }
    }

    private void AdjustSpeed(Movement movement)
    {
        if (movement.BaseSpeed > _minimalSpeed)
        {
            movement.BaseSpeed *= _speedMultyplier;
        }
        else if (movement.BaseSpeed < _minimalSpeed)
        {
            movement.BaseSpeed = _minimalSpeed;
        }
    }

    private void DeletePart()
    {
        if (_bodyParts.Count > 0)
        {
            GameObject part = _bodyParts[_bodyParts.Count - 1];
            RemoveAndDestroyPart(part);
        }
    }

    private void Death()
    {
        int partCount = Parts.Count;
        for (int i = Parts.Count - 1; i >= partCount - _mass.BeginningMass - 2; i--)
        {
            RemoveAndDestroyPart(Parts[i]);
        }

        if (Parts.Count > 0)
        {
            for (int i = Parts.Count - 1; i >= 0; i--)
            {
                GameObject part = Parts[i];
                RemoveAndDestroyPart(part);
                _foodManager.SpawnFoodItem(_foodManager.FoodOne, new Vector2(part.transform.position.x, part.transform.position.y));
            }
        }
        PhotonNetwork.Destroy(GetComponent<PlayerInput>().Camera.gameObject);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel("Lobby");
    }

    private void RemoveAndDestroyPart(GameObject part)
    {
        Parts.Remove(part);
        _bodyParts.Remove(part);
        PhotonNetwork.Destroy(part);
    }
}

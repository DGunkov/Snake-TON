using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Mass))]
[RequireComponent(typeof(Movement))]
public class Grow : MonoBehaviour
{
    private PlayerInput _input;
    private Movement _movement;
    private Mass _mass;
    private FoodManager _foodManager;

    [SerializeField] private GameObject _tail;

    private float _instantiateOffset = 0.97f;

    [SerializeField] private GameObject _bodyPart;
    [SerializeField] private float _partOffset = 0.1f;
    [SerializeField] private float _gapIncrease = 0.7f;
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
        Parts.Add(this.gameObject);
        _tail = PhotonNetwork.Instantiate(_tail.name, this.transform.position, this.transform.rotation);
        _tail.GetComponent<Tail>().Parent = Parts[Parts.Count - 1];
        Parts.Add(_tail);
        _mass = GetComponent<Mass>();
        _foodManager = FindObjectOfType<FoodManager>();
        _movement = GetComponent<Movement>();
    }

    private void GrowUp()
    {
        GameObject parent = Parts[Parts.Count - 2];
        Vector3 parentPosition = parent.transform.position;
        parentPosition.x -= _partOffset;
        GameObject bodyPart = PhotonNetwork.Instantiate(_bodyPart.name, parentPosition + (parentPosition - Vector3.one * _instantiateOffset).normalized, parent.transform.rotation) as GameObject;
        _bodyParts.Add(bodyPart);
        bodyPart.GetComponentInChildren<SpriteRenderer>().sortingOrder = -(_bodyParts.IndexOf(bodyPart));
        _tail.GetComponent<Tail>().Parent = bodyPart;
        _tail.GetComponentInChildren<SpriteRenderer>().sortingOrder = bodyPart.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;
        _tail.transform.position += (parentPosition - Vector3.one * 0.97f).normalized;

        BodyPart partScript = bodyPart.GetComponent<BodyPart>();
        Movement movement = GetComponent<Movement>();

        partScript.ParentMovement = movement;
        partScript.Parent = parent;
        Parts.Remove(_tail);
        Parts.Add(bodyPart);
        Parts.Add(_tail);

        AdjustSpeed(movement);
        AdjustRotationSpeed(movement);
        StartCoroutine(AdjustSize());

        if (_bodyParts.Count % 10 == 0 && _bodyParts.Count > 1)
        {
            AdjustGap();
        }

    }

    private void AdjustGap()
    {
        _bodyParts[0].GetComponent<BodyPart>().BasePartGap += _gapIncrease;
        _bodyParts[0].GetComponent<BodyPart>().Gap += 0.02f;
        _instantiateOffset += 0.06f;
    }

    private IEnumerator AdjustSize()
    {
        for (int i = 0; i < 5; i++)
        {
            transform.localScale *= 1 + ((_sizeMultyplier - 1) / 5);
            yield return new WaitForSeconds(0.35f);
            _tail.GetComponent<Tail>().SetScale();
        }
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
        if (GetComponent<PlayerInput>() != null)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.Destroy(GetComponent<PlayerInput>().Camera.gameObject);
            SceneManager.LoadScene("Lobby");
        }
    }

    private void RemoveAndDestroyPart(GameObject part)
    {
        Parts.Remove(part);
        _bodyParts.Remove(part);
        PhotonNetwork.Destroy(part);
    }
}

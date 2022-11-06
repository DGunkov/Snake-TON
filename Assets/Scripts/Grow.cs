using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Mass))]
[RequireComponent(typeof(Movement))]
public class Grow : MonoBehaviour
{
    private PlayerInput _input;
    private Movement _movement;
    private Mass _mass;
    private FoodManager _foodManager;

    [SerializeField] private Tail _tail;

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
        _mass.OnMassFilled += GrowUp;
        _mass.OnMassDeFilled += DeletePart;
        _movement.OnDeath += Death;
    }

    private void OnDisable()
    {
        _mass.OnMassFilled -= GrowUp;
        _mass.OnMassDeFilled -= DeletePart;
        _movement.OnDeath -= Death;
    }

    private void Awake()
    {
        Parts.Add(this.gameObject);
        _tail = GameObject.Instantiate(_tail.gameObject, this.transform.position, this.transform.rotation).GetComponent<Tail>();
        _tail.Parent = this.gameObject;
        _mass = GetComponent<Mass>();
        _movement = GetComponent<Movement>();
        _foodManager = FindObjectOfType<FoodManager>();
        if (GetComponent<PlayerInput>() != null)
        {
            _input = GetComponent<PlayerInput>();
        }
    }

    private void GrowUp()
    {
        GameObject parent = Parts[Parts.Count - 2];
        Vector3 parentPosition = parent.transform.position;
        parentPosition.x -= _partOffset;
        GameObject bodyPart = GameObject.Instantiate(_bodyPart, parentPosition, parent.transform.rotation) as GameObject;
        _bodyParts.Add(bodyPart);
        _tail.Parent = bodyPart;

        BodyPart partScript = bodyPart.GetComponent<BodyPart>();
        Movement movement = GetComponent<Movement>();

        partScript.Movement = movement;
        partScript.Parent = parent;
        Parts.Remove(Parts[Parts.Count - 1]);
        Parts.Add(bodyPart);
        Parts.Add(_tail.gameObject);

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
            Parts.Remove(part);
            _bodyParts.Remove(part);
            Destroy(part.gameObject);
        }
    }

    private void Death()
    {
        for (int i = Parts.Count - 1; i >= 0; i--)
        {
            GameObject part = Parts[i];
            Parts.Remove(part);
            _bodyParts.Remove(part);
            Destroy(part);
            _foodManager.SpawnFoodItem(_foodManager.FoodTypes[0], new Vector2(part.transform.position.x, part.transform.position.y));
        }
    }
}

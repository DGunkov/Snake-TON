using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grow : MonoBehaviour
{
    private Movement _movement;
    private PlayerInput _input;

    [SerializeField] private float _partOffset = 0.1f;
    [SerializeField] private float _minimalSpeed = 1.2f;
    [SerializeField] private float _minimalRotationSpeed = 150f;
    [SerializeField] private float _speedMultyplier = 0.98f;
    [SerializeField] private float _rotationSpeedMultyplier = 0.99f;
    [SerializeField] private float _sizeMultyplier = 1.005f;
    [SerializeField] private float _cameraMultyplier = 0.01f;
    [SerializeField] private GameObject _bodyPart;

    private List<GameObject> _parts = new List<GameObject>();

    private void OnEnable()
    {
        _movement.OnFoodEaten += GrowUp;
    }

    private void OnDisable()
    {
        _movement.OnFoodEaten -= GrowUp;
    }

    private void Awake()
    {
        _parts.Add(this.gameObject);
        _movement = GetComponent<Movement>();
        if (GetComponent<PlayerInput>() != null)
        {
            _input = GetComponent<PlayerInput>();
        }
    }

    private void GrowUp()
    {
        GameObject parent = _parts[_parts.Count - 1];
        Vector3 parentPosition = parent.transform.position;
        parentPosition.z -= _partOffset;
        GameObject bodyPart = GameObject.Instantiate(_bodyPart, parentPosition, Quaternion.identity) as GameObject;

        BodyPart part = bodyPart.GetComponent<BodyPart>();
        Movement movement = GetComponent<Movement>();

        part.Movement = movement;
        part.Parent = parent;
        _parts.Add(bodyPart);

        AdjustSpeed(movement);
        AdjustRotationSpeed(movement);

        transform.localScale *= _sizeMultyplier;

        if (_input != null)
        {
            _input.CameraZoom();
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
}

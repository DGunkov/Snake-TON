using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGrow : MonoBehaviour
{
    private Movement _movement;

    [SerializeField] private float _partOffset = 0.1f;
    [SerializeField] private float _minimalSpeed = 1.2f;
    [SerializeField] private float _minimalRotationSpeed = 60f;
    [SerializeField] private float _speedMultyplier = 0.98f;
    [SerializeField] private float _rotationSpeedMultyplier = 0.99f;
    [SerializeField] private float _sizeMultyplier = 1.005f;
    [SerializeField] private float _cameraMultyplier = 0.01f;
    [SerializeField] private GameObject _bodyPart;

    private List<GameObject> _parts = new List<GameObject>();

    private void OnEnable()
    {
        _movement.OnFoodEaten += Grow;
    }

    private void OnDisable()
    {
        _movement.OnFoodEaten -= Grow;
    }

    private void Awake()
    {
        _parts.Add(this.gameObject);
        _movement = GetComponent<Movement>();
    }

    private void Grow()
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
        if (movement.BaseSpeed >= _minimalSpeed)
        {
            movement.BaseSpeed *= _speedMultyplier;
        }
        if (movement.RotationSpeed >= _minimalRotationSpeed)
        {
            movement.RotationSpeed *= _rotationSpeedMultyplier;
        }
        transform.localScale *= _sizeMultyplier;
        StartCoroutine(CameraRemote());
    }

    private IEnumerator CameraRemote()
    {
        for (int i = 0; i < 5; i++)
        {
            Camera.main.orthographicSize += _cameraMultyplier;
            yield return new WaitForSeconds(0.35f);
        }
    }
}

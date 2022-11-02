using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Mass))]
public class PlayerInput : MonoBehaviour
{
    public bool KeyboardInput = false;
    public event Action<int> OnSnakeAppeared;

    [SerializeField] private float _cameraMultyplier = 0.01f;
    [SerializeField] private int _crystallsEntered = 100;

    private Movement _movement;
    private FoodManager _foodManager;
    private Mass _mass;

    private void OnEnable()
    {
        _mass.OnMassFilled += CameraZoom;
    }

    private void OnDisable()
    {
        _mass.OnMassFilled -= CameraZoom;
    }

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _mass = GetComponent<Mass>();
        _foodManager = FindObjectOfType<FoodManager>();
    }

    private void Start()
    {
        _foodManager.UpdateSnakes();
        OnSnakeAppeared?.Invoke(_crystallsEntered);
    }

    private void Update()
    {
        RotationInput();
    }

    private void RotationInput()
    {
#if UNITY_STANDALONE
        if (KeyboardInput)
        {
            float direction = -Input.GetAxisRaw("Horizontal");
            _movement.Rotate(direction);
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            _movement.Rotate(direction);
        }
#endif
    }

    public void CameraZoom()
    {
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

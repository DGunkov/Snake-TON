using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Mass))]
public class PlayerInput : MonoBehaviour
{
    public int CrystallsEntered = 100;
    public GameObject Camera;
    public event Action<int> OnSnakeAppeared;

    [SerializeField] private float _cameraMultyplier = 0.01f;

    private Mass _mass;
    private PhotonView _view;
    private Movement _movement;
    private Joystick _joystick;
    private FoodManager _foodManager;
    private bool _keyboardInput = false;

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
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
        {
            _movement = GetComponent<Movement>();
            _mass = GetComponent<Mass>();
            _foodManager = FindObjectOfType<FoodManager>();
            _joystick = FindObjectOfType<Joystick>();
            if (DataHolder.MouseInput)
            {
                _keyboardInput = !DataHolder.MouseInput;
            }
            else if (!DataHolder.MouseInput)
            {
                _keyboardInput = !DataHolder.MouseInput;
            }
#if UNITY_ANDROID || UNITY_IOS
            _joystick.gameObject.SetActive(true);
#endif
        }
    }

    private void Start()
    {
        _foodManager.UpdateSnakes();
        OnSnakeAppeared?.Invoke(CrystallsEntered);
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            RotationInput();
            _movement.Move();
        }
    }

    private void RotationInput()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        if (_keyboardInput)
        {
            float direction = -Input.GetAxisRaw("Horizontal");
            _movement.Rotate(direction);
        }
        else
        {
            Vector3 mousePosition = Camera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            _movement.Rotate(direction);
        }
        if (_joystick != null)
        {
            _joystick.gameObject.SetActive(false);
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        if (_joystick != null)
        {
            float direction = -_joystick.Horizontal;
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
            Camera.GetComponent<Camera>().orthographicSize += _cameraMultyplier;
            yield return new WaitForSeconds(0.35f);
        }
    }

}

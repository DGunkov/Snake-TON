using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Mass))]
public class PlayerInput : MonoBehaviourPunCallbacks
{
    public int CrystallsEntered = 100;
    public GameObject Camera;
    public event Action<int> OnSnakeAppeared;
    internal bool _mainPlayer;

    [SerializeField] private float _cameraMultyplier = 0.01f;

    private Grow _grow;
    private Mass _mass;
    private PhotonView _view;
    private Movement _movement;
    private Joystick _joystick;
    private FoodManager _foodManager;
    private bool _keyboardInput = false;

    private AudioSource _audio_source;
    [SerializeField] AudioClip[] _sounds;

    public override void OnEnable()
    {
        _view = GetComponent<PhotonView>();
        base.OnEnable();
        if(_mass == null)
        {
            _mass = GetComponent<Mass>();
        }
        if (_view.IsMine)
        {
            _mass.OnMassFilled += CameraZoom;
        }
    }

    public override void OnDisable()
    {
        _view = GetComponent<PhotonView>();
        base.OnDisable();
        if (_view.IsMine)
        {
            _mass.OnMassFilled -= CameraZoom;
        }
    }

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _mass = GetComponent<Mass>();
        _grow = GetComponent<Grow>();
        _movement = GetComponent<Movement>();
        if (_view.IsMine)
        {
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
        else
        {
            _mass.enabled = false;
            _movement.enabled = false;
            _grow.enabled = false;
            this.enabled = false;
        }
    }

    private void Start()
    {
        _audio_source = Camera.GetComponentInChildren<AudioSource>();
        if (_foodManager == null)
        {
            _foodManager = FindObjectOfType<FoodManager>();
        }
        _foodManager.UpdateSnakes();
        base.photonView.RPC("RPC_SpawnFood", RpcTarget.AllBuffered, CrystallsEntered);
    }
    internal void PlaySound(int n)
    {
        _audio_source.clip = _sounds[n];
        _audio_source.Play();
    }

    [PunRPC]
    private void RPC_SpawnFood(int foodMass)
    {
        OnSnakeAppeared?.Invoke(foodMass);
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            RotationInput();
        }
    }

    private void RotationInput()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        if (_keyboardInput)
        {
           /* float direction = -Input.GetAxisRaw("Horizontal");
            _movement.direction = direction;*/
        }
        else
        {
            Vector3 mousePosition = Camera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            _movement.direction = direction;
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
        if (_view.IsMine)
        {
            StartCoroutine(CameraRemote());
        }
    }

    private IEnumerator CameraRemote()
    {
        for (int i = 0; i < 5; i++)
        {
            Camera.GetComponent<Camera>().orthographicSize += _cameraMultyplier;
            DataHolder.RenderDistance += 0.02f;
            yield return new WaitForSeconds(0.35f);
        }
    }

}

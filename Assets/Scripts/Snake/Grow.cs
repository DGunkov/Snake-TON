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
    private float _gap = 2;
    internal bool _sprint;
    private bool _next_spawn;
    private Vector3 _start_scale;
    private int _count_spawn_parts;
    private float _time_to_new_spawn = 1;
    private bool _exit_button = false;

    private float _instantiateOffset = 0.97f;

    [SerializeField] private GameObject _bodyPart;
    [SerializeField] private float _partOffset = 0.1f;
    [SerializeField] private float _gapIncrease = 0.7f;
    [SerializeField] private float _minimalSpeed = 1.2f;
    [SerializeField] private float _speedMultyplier = 0.98f;
    [SerializeField] private float _sizeMultyplier = 2f;
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
            _mass = GetComponent<Mass>();
            _foodManager = FindObjectOfType<FoodManager>();
            _movement = GetComponent<Movement>();
        }
    }
    private void Start()
    {
        _start_scale = transform.localScale;
    }

    private void Update()
    {
        float distance = 0;
        if (Parts.Count > 1)
        {
            distance = ((Vector2)Parts[1].transform.position - (Vector2)Parts[0].transform.position).magnitude;
        }

        for (int n = 1; n < Parts.Count; n++)
        {
            if(_sprint)
            {
                Parts[n].transform.position = Vector2.Lerp(Parts[n].transform.position, Parts[n - 1].transform.position, distance / (_gap / (_movement._sprintMultyplier * 0.62f)));
            }
            else
            {
                Parts[n].transform.position = Vector2.Lerp(Parts[n].transform.position, Parts[n - 1].transform.position, distance / _gap);
            }
        }
        if (_time_to_new_spawn > 0)
        {
            _time_to_new_spawn -= Time.deltaTime;
        }
        else
        {
            if (_count_spawn_parts > 0)
            {
                StartCoroutine(GrowUpCor());
                _count_spawn_parts--;
                _time_to_new_spawn = 1;
            }            
        }
        
    }

    private void GrowUp()
    {
        _count_spawn_parts++;
    }

    private IEnumerator GrowUpCor()
    {
        yield return null;
        GameObject parent = Parts[Parts.Count - 1];
        Vector3 parentPosition = parent.transform.position;
        parentPosition.x -= _partOffset;
        GameObject bodyPart = PhotonNetwork.Instantiate(_bodyPart.name, parentPosition + (parentPosition - Vector3.one * _instantiateOffset).normalized, parent.transform.rotation) as GameObject;
        _bodyParts.Add(bodyPart);
        bodyPart.GetComponentInChildren<SpriteRenderer>().sortingOrder = -(_bodyParts.IndexOf(bodyPart));

        BodyPart partScript = bodyPart.GetComponent<BodyPart>();

        partScript.ParentMovement = _movement;
        partScript.Parent = parent;
        Parts.Add(bodyPart);

        if(_start_scale.x * 3 > transform.localScale.x)
        {
            StartCoroutine(AdjustSize());
            AdjustGap();
            AdjustRotationSpeed();
        }
    }

    private void AdjustGap()
    {
        _instantiateOffset += 0.06f;
    }

    private IEnumerator AdjustSize()
    {
        for (int i = 0; i < 5; i++)
        {
            transform.localScale *= 1 + ((_sizeMultyplier - 1) / 2);
            _gap *= _sizeMultyplier;
            _movement.RotationSpeed /= 1 + ((_sizeMultyplier - 1) / 2);
            yield return new WaitForSeconds(0.35f);
        }
    }

    private void AdjustRotationSpeed()
    {
        if (_movement.RotationSpeed > _minimalRotationSpeed)
        {
            _movement.RotationSpeed *= _rotationSpeedMultyplier;
        }
        else if (_movement.RotationSpeed < _minimalRotationSpeed)
        {
            _movement.RotationSpeed = _minimalRotationSpeed;
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

    public void Death()
    {
        if (GetComponent<PlayerInput>() != null)
        {
            DisableSnake();
            GetComponent<PlayerInput>().UiManager.SwitchDeatchPanel(true);
        }
        else
        {
            StartDestroyParts();
        }
    }

    public void StartDestroyParts()
    {
        int partCount = Parts.Count;
        for (int i = Parts.Count - 1; i >= partCount - _mass.BeginningMass - 1 && i > 0; i--)
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
    }

    void DisableSnake()
    {
        GetComponent<Movement>().enabled = false;
        for (int i = 0; i < Parts.Count; i++)
        {
            if (Parts[i] == gameObject)
            {
                transform.Find("HeadSprite").gameObject.SetActive(false);
                transform.Find("Navigation").gameObject.SetActive(false);
            }
            else
            {
                Parts[i].SetActive(false);
            }
        }
    }

    public void PressExit()
    {
        DisableSnake();
        GetComponent<PlayerInput>().UiManager.Stats();
    }

    public void Exit()
    {
        StartDestroyParts();
        DataHolder.ClearList();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.Destroy(GetComponent<PlayerInput>().Camera.gameObject);
        SceneManager.LoadScene("Lobby");
    }

    public void Reborn()
    {
        GetComponent<PlayerInput>().UiManager.SwitchDeatchPanel(false);
        for (int i = 0; i < Parts.Count; i++)
        {
            if (Parts[i] == gameObject)
            {
                transform.Find("HeadSprite").gameObject.SetActive(true);
                transform.Find("Navigation").gameObject.SetActive(true);
                GetComponent<Movement>().enabled = true;
            }
            else
            {
                float randAng = UnityEngine.Random.Range(0, Mathf.PI * 2);
                transform.position = new Vector2(Mathf.Cos(randAng) * 50, Mathf.Sin(randAng) * 50);
                Parts[i].transform.position = transform.position;
                Parts[i].SetActive(true);
            }
        }            
    }

    private void RemoveAndDestroyPart(GameObject part)
    {
        Parts.Remove(part);
        _bodyParts.Remove(part);
        PhotonNetwork.Destroy(part);
    }
}

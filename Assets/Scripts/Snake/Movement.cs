using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[RequireComponent(typeof(Mass))]
[RequireComponent(typeof(Grow))]
public class Movement : MonoBehaviourPunCallbacks
{
    public event Action<float> OnFoodEatenLocal;
    public static event Action<GameObject> OnFoodEatenGlobal;
    public Action OnDeath;
    public float Speed;
    internal float BaseSpeed;
    public float RotationSpeed;
    public float MaxEnergy;
    public float Energy;
    internal Vector2 direction;
    private Vector3 lastPos;
    private bool start_death;

    internal bool danger;


    [SerializeField] internal float _sprintMultyplier;
    [SerializeField] private float _energyChange;
    [SerializeField] private float _baseAnimationDelay = 30f;

    private bool _isAnimating = false;
    private GameObject _lastFoodEaten;

    private UiManager UiManager;
    private Mass _mass;
    private Grow _grow;
    private PlayerInput PlayerInput;

    private void OnEnable()
    {
        if (GetComponent<PhotonView>().IsMine && this.gameObject.tag.Equals("Player"))
        {
            _mass = GetComponent<Mass>();
            _grow = GetComponent<Grow>();
            PlayerInput = GetComponent<PlayerInput>();
            Energy = MaxEnergy;
        }
    }

    private void Awake()
    {
        BaseSpeed = Speed;
        if (GetComponent<PhotonView>().IsMine && this.gameObject.tag.Equals("Player"))
        {
            _mass = GetComponent<Mass>();
            _grow = GetComponent<Grow>();
            PlayerInput = GetComponent<PlayerInput>();
            Energy = MaxEnergy;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.left * Speed * Time.deltaTime, Space.Self);
        //Debug.Log(Vector3.Distance(lastPos, transform.position));
        lastPos = transform.position;

        if(danger)
        {
            Vector3 new_direction = -transform.right - transform.position;
            float angle = Vector2.SignedAngle(Vector2.left, new_direction);
            Vector3 targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), RotationSpeed * Time.deltaTime);
        }
        else
        {
            float angle = Vector2.SignedAngle(Vector2.left, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), RotationSpeed * Time.deltaTime);
        }

        if (this.gameObject.tag.Equals("Player") && Input.GetMouseButton(0) && (_mass._weight > 0 || Energy > 0) && GetComponent<PhotonView>().IsMine)
        {
            Sprint();
        }
        else Unsprint();
    }

    private void Sprint()
    {
        _grow._sprint = true;
        Speed = BaseSpeed * _sprintMultyplier;
        WasteEnergy();
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetBool("IsSprinting", true);
        }
        if (!_isAnimating)
        {
            _isAnimating = true;
            StartCoroutine(SprintAnimation());
        }
    }

    private IEnumerator SprintAnimation()
    {
        float delay = _baseAnimationDelay / _grow.Parts.Count;
        while (_isAnimating)
        {
            for (int i = 0; i <= _grow.Parts.Count - 1; i++)
            {
                if (i > 1)
                {
                    _grow.Parts[i - 1].GetComponentInChildren<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
                    _grow.Parts[i - 2].GetComponentInChildren<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
                }
                Color exColor = _grow.Parts[i].GetComponentInChildren<SpriteRenderer>().color;
                _grow.Parts[i].GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1);
                if (i < _grow.Parts.Count - 1)
                {
                    _grow.Parts[i + 1].GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1);
                }
                if (i == 0)
                {
                    _grow.Parts[_grow.Parts.Count - 1].GetComponentInChildren<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
                }
                delay = _baseAnimationDelay / _grow.Parts.Count;
                yield return new WaitForSeconds(delay);
            }
            _grow.Parts[_grow.Parts.Count - 1].GetComponentInChildren<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
        }
        for (int i = 0; i <= _grow.Parts.Count - 1; i++)
        {
            if (i % 2 == 1)
            {
                _grow.Parts[i].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
            }
        }
    }

    private void Unsprint()
    {
        if(_grow != null)
        {
            _grow._sprint = false;
        }
        if (SprintKeyUp() || Speed != BaseSpeed)
        {
            Speed = BaseSpeed;
            if(_grow == null)
            {
                _grow = GetComponent<Grow>();
            }
            //_grow.Parts[1].GetComponent<BodyPart>().PartGap = _grow.Parts[1].GetComponent<BodyPart>().BasePartGap;
        }
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)))
        {
            RecoverEnegry();
        }

        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetBool("IsSprinting", false);
        }
        _isAnimating = false;
        StopCoroutine(SprintAnimation());
    }

    private static bool SprintKeyUp()
    {
        return Input.GetKeyUp(KeyCode.LeftShift) || Input.GetMouseButtonUp(0);
    }

    private void RecoverEnegry()
    {
        if (Energy < MaxEnergy && !(Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)))
        {
            Energy += _energyChange / 2 * Time.deltaTime;
        }

        if (Energy > MaxEnergy)
        {
            Energy = MaxEnergy;
        }
    }

    private void WasteEnergy()
    {
        Energy -= _energyChange * Time.deltaTime;

        if (Energy < 0)
        {
            Energy = 0;
            _mass.SubstractMass(Time.deltaTime / 2);
        }
    }

    internal void Trigger(Collider2D other)
    {
        if (other.tag.Equals("Food"))
        {
            if(PlayerInput != null)
            {
                PlayerInput.PlaySound(0);
                PlayerInput._collect_crystalls++;
            }
            OnFoodEatenLocal?.Invoke(other.gameObject.GetComponent<Food>().Satiety);
            _lastFoodEaten = other.gameObject;
            base.photonView.RPC("RPC_FoodEaten", RpcTarget.AllBuffered);
            RPC_FoodEaten();

        }
        else
        {
            if(PlayerInput != null)
            {
                if (_grow != null && !_grow.Parts.Contains(other.gameObject) && other.tag.Equals("NPC"))
                {
                    start_death = true;
                    OnDeath?.Invoke();
                }
            }
            else
            {
                if (_grow != null && !_grow.Parts.Contains(other.gameObject))
                {
                    if (!other.tag.Equals("NPC") && !start_death)
                    {
                        other.GetComponent<BodyPart>().ParentMovement.gameObject.GetComponent<PlayerInput>()._kills++;
                    }
                    start_death = true;
                    OnDeath?.Invoke();
                }
            }
            
        }
    }

    [PunRPC]
    private void RPC_FoodEaten()
    {
        OnFoodEatenGlobal?.Invoke(_lastFoodEaten);
    }
}

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
    public float Speed = 3f;
    public float BaseSpeed;
    public float RotationSpeed = 90f;
    public float MaxEnergy;
    public float Energy;

    [SerializeField] private float _sprintMultyplier;
    [SerializeField] private float _energyChange;
    [SerializeField] private float _baseAnimationDelay = 30f;

    private bool _isAnimating = false;
    private GameObject _lastFoodEaten;

    private Mass _mass;
    private Grow _grow;

    private void Awake()
    {
        if (GetComponent<PhotonView>().IsMine && this.gameObject.tag.Equals("Player"))
        {
            BaseSpeed = Speed;
            _mass = GetComponent<Mass>();
            _grow = GetComponent<Grow>();
            Energy = MaxEnergy;
        }
        else
        {
            if(!GetComponent<PhotonView>().IsMine)
            {
                this.enabled = false;
            }
        }

        if (GetComponent<NPC>() != null)
        {
            BaseSpeed = Speed;
            _mass = GetComponent<Mass>();
            _grow = GetComponent<Grow>();
            Energy = MaxEnergy;
        }
    }

    private void Update()
    {
        if (CanSprint())
        {
            Sprint();
        }
        else Unsprint();
    }

    private bool CanSprint()
    {
        return (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)) && (_mass.Weight > 0 || Energy > 0) && GetComponent<PhotonView>().IsMine && GetComponent<NPC>() == null;
    }

    private void Sprint()
    {
        Speed = BaseSpeed * _sprintMultyplier;
        _grow.Parts[1].GetComponent<BodyPart>().PartGap = _grow.Parts[1].GetComponent<BodyPart>().BasePartGap / _sprintMultyplier;
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
        if (SprintKeyUp() || Speed != BaseSpeed)
        {
            Speed = BaseSpeed;
            if(_grow == null)
            {
                _grow = GetComponent<Grow>();
            }
            _grow.Parts[1].GetComponent<BodyPart>().PartGap = _grow.Parts[1].GetComponent<BodyPart>().BasePartGap;
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

    public void Rotate(float direction)
    {
        transform.rotation *= Quaternion.Euler(transform.rotation.x, transform.rotation.y, direction * RotationSpeed * Time.deltaTime);
    }

    public void Rotate(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.left, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), RotationSpeed * Time.deltaTime);
    }

    public void Move()
    {
        transform.Translate(Vector3.left * Speed * Time.deltaTime, Space.Self);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Food"))
        {
            OnFoodEatenLocal?.Invoke(other.gameObject.GetComponent<Food>().Satiety);
            _lastFoodEaten = other.gameObject;
            base.photonView.RPC("RPC_FoodEaten", RpcTarget.AllBuffered);
        }
        if ((other.tag.Equals("Obstacle") || other.tag.Equals("Snake") || other.tag.Equals("Player")) && !_grow.Parts.Contains(other.gameObject))
        {
            OnDeath?.Invoke();
        }
    }*/

    [PunRPC]
    private void RPC_FoodEaten()
    {
        //OnFoodEatenGlobal?.Invoke(_lastFoodEaten);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Mass))]
[RequireComponent(typeof(Grow))]
public class Movement : MonoBehaviour
{
    public static event Action<GameObject> OnFoodEatenGlobal;
    public event Action<float> OnFoodEatenLocal;
    public Action OnDeath;
    public float Speed = 3f;
    public float BaseSpeed;
    public float RotationSpeed = 90f;

    [SerializeField] private float _sprintMultyplier;
    [SerializeField] private float _energy;
    [SerializeField] private float _maxEnergy;
    [SerializeField] private float _energyChange;

    private Mass _mass;
    private Grow _grow;

    private void Start()
    {
        BaseSpeed = Speed;
        _mass = GetComponent<Mass>();
        _grow = GetComponent<Grow>();
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
        return (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)) && (_mass.Weight > 0 || _energy > 0);
    }

    private void Sprint()
    {
        Speed = BaseSpeed * _sprintMultyplier;
        WasteEnergy();
    }

    private void Unsprint()
    {
        if (SprintKeyUp() || Speed != BaseSpeed)
        {
            Speed = BaseSpeed;
        }
        if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)))
        {
            RecoverEnegry();
        }
    }

    private static bool SprintKeyUp()
    {
        return Input.GetKeyUp(KeyCode.LeftShift) || Input.GetMouseButtonUp(0);
    }

    private void RecoverEnegry()
    {

        if (_energy < _maxEnergy && !(Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)))
        {
            _energy += _energyChange / 2 * Time.deltaTime;
        }

        if (_energy > _maxEnergy)
        {
            _energy = _maxEnergy;
        }
    }

    private void WasteEnergy()
    {
        _energy -= _energyChange * Time.deltaTime;

        if (_energy < 0)
        {
            _energy = 0;
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
        transform.Translate(Vector3.left * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Food"))
        {
            OnFoodEatenGlobal?.Invoke(other.gameObject);
            OnFoodEatenLocal?.Invoke(other.gameObject.GetComponent<Food>().Satiety);
            Destroy(other.gameObject);
        }
        if (other.tag.Equals("Obstacle") || (other.tag.Equals("Snake") && !_grow.Parts.Contains(other.gameObject)))
        {
            OnDeath?.Invoke();
        }
    }
}

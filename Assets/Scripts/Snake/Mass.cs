using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Mass : MonoBehaviour
{
    public Action OnMassFilled;
    public Action OnMassDeFilled;

    private Movement _movement;
    internal float _weight;
    public float BeginningMass;

    private void OnEnable()
    {
        _movement.OnFoodEatenLocal += AddMass;
    }

    private void OnDisable()
    {
        _movement.OnFoodEatenLocal -= AddMass;
    }

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Start()
    {
        for (int i = 0; i < BeginningMass; i++)
        {
            AddMass(1);
            _weight = 0;
        }
    }

    public void AddMass(float value)
    {
        if (_weight < 0)
        {
            _weight = 0;
        }
        float remain = _weight % 2;
        _weight += value;
        CheckForFill(remain, OnMassFilled);
    }

    public void SubstractMass(float value)
    {
        if (_weight > 0)
        {
            float remain = _weight % 2;
            _weight -= value;
            CheckForFill(remain, OnMassDeFilled);
        }
        if (_weight < 0)
        {
            _weight = 0;
            return;
        }
    }

    private void CheckForFill(float remain, Action action)
    {
        if (remain < 1 && _weight % 2 >= 1)
        {
            action?.Invoke();
        }

        if (remain > 1 && (_weight % 2 < 1 || _weight % 2 == 0))
        {
            action?.Invoke();
        }
    }
}

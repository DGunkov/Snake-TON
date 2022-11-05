using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class NPC : MonoBehaviour
{
    private List<GameObject> _allFood = new List<GameObject>();

    private Movement _movement;
    private FoodManager _foodManager;
    private Vector2 _direction;
    private GameObject _closestFood;

    private void OnEnable()
    {
        _movement.OnFoodEaten += RemoveFoodFromList;
        _foodManager.OnFoodSpawned += FillList;
    }

    private void OnDisable()
    {
        _movement.OnFoodEaten -= RemoveFoodFromList;
        _foodManager.OnFoodSpawned -= FillList;
    }

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _foodManager = FindObjectOfType<FoodManager>();
    }

    private void Update()
    {
        _movement.Move();
        if (_closestFood != null)
        {
            _direction = _closestFood.transform.position - transform.position;
            _movement.Rotate(_direction);
        }
    }

    private void FillList(GameObject food)
    {
        _allFood.Add(food);
        GetClosestFood();
    }

    private void GetClosestFood()
    {
        _closestFood = _allFood[0];
        for (int i = 0; i < _allFood.Count - 1; i++)
        {
            GameObject currentFood = _allFood[i];
            if (GetDistance(currentFood) < GetDistance(_closestFood))
            {
                _closestFood = currentFood;
            }
        }
    }

    private float GetDistance(GameObject to)
    {
        return Vector3.Distance(to.transform.position, transform.position);
    }

    private void RemoveFoodFromList(GameObject food)
    {
        _allFood.Remove(food.gameObject);
        if (_allFood.Count > 0)
        {
            GetClosestFood();
        }
    }
}

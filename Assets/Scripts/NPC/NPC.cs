using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class NPC : MonoBehaviour
{
    [SerializeField] private List<GameObject> _allFood = new List<GameObject>();

    private Movement _movement;
    private FoodManager _foodManager;
    private Vector2 _direction;
    private GameObject _food;

    private void OnEnable()
    {
        Movement.OnFoodEatenGlobal += RemoveFoodFromList;
        _foodManager.OnFoodSpawned += FillList;
    }

    private void OnDisable()
    {
        Movement.OnFoodEatenGlobal -= RemoveFoodFromList;
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
        if (_food != null)
        {
            _direction = _food.transform.position - transform.position;
            _movement.Rotate(_direction);
        }
    }

    private void FillList(GameObject food)
    {
        _allFood.Add(food);
        GetRandomFood();
    }

    private void GetRandomFood()
    {
        _food = _allFood[Random.Range(0, _allFood.Count)];
    }

    private float GetDistance(GameObject to)
    {
        if (to != null)
        {
            return Vector3.Distance(to.transform.position, transform.position);
        }
        else return Mathf.Infinity;
    }

    private void RemoveFoodFromList(GameObject food)
    {
        _allFood.Remove(food.gameObject);
        if (_allFood.Count > 0)
        {
            GetRandomFood();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class NPC : MonoBehaviour
{
    [SerializeField] private List<GameObject> _allFood = new List<GameObject>();

    private Movement _movement;
    private Vector2 _direction;
    private GameObject _food;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        _movement.Move();
        if (_food != null)
        {
            _direction = _food.transform.position - transform.position;
            _movement.Rotate(_direction);
        }
        else if (_food == null)
        {
            _food = DataHolder.GetRandomFood();
        }
    }

    private void FillList(GameObject food)
    {
        _allFood.Add(food);
        GetRandomFood();
    }

    private void GetRandomFood()
    {
        _food = DataHolder.AllFood[Random.Range(0, DataHolder.AllFood.Count)];
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

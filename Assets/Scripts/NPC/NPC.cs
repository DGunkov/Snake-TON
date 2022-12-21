using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Movement))]
public class NPC : MonoBehaviour
{
    [SerializeField] private List<GameObject> _allFood = new List<GameObject>();

    private Grow _grow;
    private Mass _mass;
    private PhotonView _view;
    private Movement _movement;
    private Vector2 _direction;
    private GameObject _food;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _mass = GetComponent<Mass>();
        _grow = GetComponent<Grow>();
        _movement = GetComponent<Movement>();

        if (!_view.IsMine)
        {
            _mass.enabled = false;
            _movement.enabled = false;
            _grow.enabled = false;
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (_food != null)
        {
            _direction = _food.transform.position - transform.position;
            _movement.direction = _direction;
        }
        else 
        {
            if (DataHolder.AllFood.Count > 0)
            {
                float distance_to_food = 100000;
                foreach (GameObject obj in DataHolder.AllFood)
                {
                    if (distance_to_food > Vector3.Distance(transform.position, obj.transform.position))
                    {
                        distance_to_food = Vector3.Distance(transform.position, obj.transform.position);
                        _food = obj;
                    }
                }
            }
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

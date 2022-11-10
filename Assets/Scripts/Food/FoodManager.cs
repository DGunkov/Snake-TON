using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class FoodManager : MonoBehaviour
{
    public int AllFoodMass;
    public Food FoodOne;
    public Food FoodHalf;
    public Food FoodThird;
    public event Action<GameObject> OnFoodSpawned;

    [SerializeField] private float _width = 20.0f;
    [SerializeField] private float _height = 10.0f;
    [SerializeField] private List<PlayerInput> _players;


    private void OnEnable()
    {
        foreach (PlayerInput player in _players)
        {
            player.OnSnakeAppeared += SpawnAllFood;
        }
    }

    private void OnDisable()
    {
        foreach (PlayerInput player in _players)
        {
            player.OnSnakeAppeared -= SpawnAllFood;
        }
    }

    public void UpdateSnakes()
    {
        _players.Clear();
        foreach (PlayerInput player in FindObjectsOfType<PlayerInput>())
        {
            _players.Add(player);
        }
        OnEnable();
    }

    void Start()
    {

    }

    private void SpawnAllFood(int foodMass)
    {
        AllFoodMass += foodMass;
        if (foodMass % 2 == 0)
        {
            for (int i = 0; i < foodMass / 2; i++)
            {
                SpawnFood(3, FoodThird);
                SpawnFood(2, FoodHalf);
            }
        }
        else
        {
            for (int i = 0; i < (foodMass - 1) / 2; i++)
            {
                SpawnFood(3, FoodThird);
                SpawnFood(2, FoodHalf);
            }
            SpawnFood(2, FoodHalf);
        }
    }

    private void SpawnFood(int count, Food type)
    {
        for (int j = 0; j < count; j++)
        {
            SpawnFoodItem(type, GetRandomPosition());
        }
    }

    public void SpawnFoodItem(Food foodType, Vector2 position)
    {
        Vector3 _spawnPosition = new Vector3(position.x, position.y, 0);
        GameObject food = PhotonNetwork.InstantiateRoomObject(foodType.name, _spawnPosition, Quaternion.identity);
        OnFoodSpawned?.Invoke(food);
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(UnityEngine.Random.Range(_width * -1, _width), UnityEngine.Random.Range(_height * -1, _height));
    }
}

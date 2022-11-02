using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public List<FoodItem> FoodTypes;
    public int AllFoodMass;

    [SerializeField] private List<PlayerInput> _players;
    [SerializeField] private Food _food;
    [SerializeField] private float _width = 20.0f;
    [SerializeField] private float _height = 10.0f;

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
                SpawnFood(3, FoodTypes[2]);
                SpawnFood(2, FoodTypes[1]);
            }
        }
        else
        {
            for (int i = 0; i < (foodMass - 1) / 2; i++)
            {
                SpawnFood(3, FoodTypes[2]);
                SpawnFood(2, FoodTypes[1]);
            }
            SpawnFood(2, FoodTypes[1]);
        }
    }

    private void SpawnFood(int count, FoodItem type)
    {
        for (int j = 0; j < count; j++)
        {
            SpawnFoodItem(type, GetRandomPosition());
        }
    }

    public void SpawnFoodItem(FoodItem foodType, Vector2 position)
    {
        _food.Item = foodType;
        Vector3 _spawnPosition = new Vector3(position.x, position.y, 0);
        GameObject.Instantiate(_food, _spawnPosition, Quaternion.identity);
    }

    private Vector2 GetRandomPosition()
    {
        return new Vector2(Random.Range(_width * -1, _width), Random.Range(_height * -1, _height));
    }
}

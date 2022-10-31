using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject _food;
    private float width = 8.0f;
    private float height = 4.0f;
    [SerializeField] private List<Movement> _snakes;

    private void OnEnable()
    {
        foreach (Movement snake in _snakes)
        {
            snake.OnFoodEaten += Add;
            snake.OnEnabled += UpdateSnakes;
        }
    }

    private void OnDisable()
    {
        foreach (Movement snake in _snakes)
        {
            snake.OnFoodEaten -= Add;
            snake.OnEnabled -= UpdateSnakes;
        }
    }

    private void UpdateSnakes()
    {
        _snakes.Clear();
        foreach (Movement snake in FindObjectsOfType<Movement>())
        {
            _snakes.Add(snake);
        }
        OnEnable();
    }

    void Start()
    {
        UpdateSnakes();
        for (int i = 0; i < 7; ++i)
        {
            Add();
        }
    }

    public void Add()
    {
        Vector3 pos = new Vector3(Random.Range(width * -1, width), Random.Range(height * -1, height), 0);
        GameObject.Instantiate(_food, pos, Quaternion.identity);
    }
}

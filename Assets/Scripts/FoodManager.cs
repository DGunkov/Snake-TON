using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject _food;
    private float width = 8.0f;
    private float height = 4.0f;
    public List<Movement> Snakes;

    private void OnEnable()
    {
        foreach (Movement snake in Snakes)
        {
            snake.OnFoodEaten += Add;
        }
    }

    private void OnDisable()
    {
        foreach (Movement snake in Snakes)
        {
            snake.OnFoodEaten -= Add;
        }
    }

    public void UpdateSnakes()
    {
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
        if (FindObjectsOfType<FoodItem>().Length < 10)
        {
            Vector3 pos = new Vector3(Random.Range(width * -1, width), Random.Range(height * -1, height), 0);
            GameObject.Instantiate(_food, pos, Quaternion.identity);
        }
    }
}

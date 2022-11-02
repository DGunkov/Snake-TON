using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform _snake;

    private void Awake()
    {
        _snake = FindObjectOfType<PlayerInput>().transform;
    }

    private void Update()
    {
        if (_snake != null)
        {
            transform.position = new Vector3(_snake.position.x, _snake.position.y, transform.position.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyPart : MonoBehaviour
{
    [SerializeField] private Transform _bodyPartGfx;
    [SerializeField] private float _partRadius;

    private List<Transform> _snakeBody = new List<Transform>();
    private List<Vector2> _positions = new List<Vector2>();

    private void Awake()
    {
        _positions.Add(_bodyPartGfx.position);
    }

    private void Update()
    {
        float distance = ((Vector2)_bodyPartGfx.position - _positions[0]).magnitude;

        if (distance > _partRadius)
        {
            Vector2 direction = ((Vector2)_bodyPartGfx.position - _positions[0]).normalized;

            _positions.Insert(0, _positions[0] + direction * _partRadius);
            _positions.RemoveAt(_positions.Count - 1);

            distance -= _partRadius;
        }

        MoveParts(distance);
    }

    private void MoveParts(float distance)
    {
        for (int i = 0; i < _snakeBody.Count; i++)
        {
            _snakeBody[i].position = Vector2.Lerp(_positions[i + 1], _positions[i], distance / _partRadius);
        }
    }

    public void AddPart()
    {
        Transform part = Instantiate(_bodyPartGfx, _positions[_positions.Count - 1], Quaternion.identity, transform);
        _snakeBody.Add(part);
        _positions.Add(part.position);
    }
}

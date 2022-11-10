using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float Satiety;
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Color _color;

    private void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = _sprite;
        GetComponentInChildren<SpriteRenderer>().color = _color;
        _animator = GetComponent<Animator>();
        _animator.speed = Random.Range(0.5f, 2f);
        transform.localScale = Vector3.one * Satiety * 3;
    }
}

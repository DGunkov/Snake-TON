using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public FoodItem Item;
    public float Satiety { get; private set; }
    public Animator _animator;

    private void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = Item.Sprite;
        GetComponentInChildren<SpriteRenderer>().color = Item.Color;
        _animator = GetComponent<Animator>();
        _animator.speed = Random.Range(0.5f, 2f);
        Satiety = Item.Satiety;
        transform.localScale = Vector3.one * Item.Satiety * 3;
    }
}

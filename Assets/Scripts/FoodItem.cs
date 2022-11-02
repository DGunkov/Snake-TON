using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New food", menuName = "ScriptableObjects/Food")]
public class FoodItem : ScriptableObject
{
    public Sprite Sprite;
    public float Satiety;
}

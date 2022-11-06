using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public GameObject Parent;
    public Movement Movement;

    [SerializeField] private float _partsGap = 2f;

    private FoodManager _foodManager;


    private void Update()
    {
        Rotate();
        transform.position = Vector3.Lerp(transform.position, Parent.transform.position, Time.deltaTime * Movement.Speed * _partsGap);
        transform.localScale = Parent.transform.localScale;
    }

    private void Awake()
    {
        _foodManager = FindObjectOfType<FoodManager>();
    }

    private void Rotate()
    {
        Vector2 direction = Parent.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.down, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Movement.RotationSpeed * Time.deltaTime);
    }

    // private void OnDestroy()
    // {
    //     _foodManager.SpawnFoodItem(_foodManager.FoodTypes[0], new Vector2(transform.position.x, transform.position.y));
    // }
}

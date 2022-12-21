using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private Movement Movement;
    void Start()
    {
        Movement = transform.parent.GetComponent<Movement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Movement.Trigger(other);
    }
}

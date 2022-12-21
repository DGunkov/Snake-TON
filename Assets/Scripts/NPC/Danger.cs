using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    private Movement Movement;
    void Start()
    {
        Movement = transform.parent.GetComponent<Movement>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.tag.Equals("Food"))
        {
            Movement.danger = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.tag.Equals("Food"))
        {
            Movement.danger = false;
        }
    }
}

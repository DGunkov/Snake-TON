using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public GameObject Parent;
    public Movement ParentMovement;

    [SerializeField] public float PartGap = 5f;


    private void Update()
    {
        if (Parent != null)
        {
            Move();
            SetScale();
            SetGap();
            Rotate();
        }
    }

    private void SetGap()
    {
        if (Parent.GetComponent<BodyPart>() != null && PartGap != Parent.GetComponent<BodyPart>().PartGap)
        {
            PartGap = Parent.GetComponent<BodyPart>().PartGap;
        }
    }

    private void SetScale()
    {
        if (transform.localScale != Parent.transform.localScale)
        {
            transform.localScale = Parent.transform.localScale;
        }
    }

    private void Move()
    {
        float distance = ((Vector2)transform.position - (Vector2)Parent.transform.position).magnitude;
        transform.position = Vector2.Lerp(transform.position, Parent.transform.position, distance / PartGap);
    }

    private void Rotate()
    {
        Vector2 direction = Parent.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.down, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), ParentMovement.RotationSpeed * Time.deltaTime);
    }
}

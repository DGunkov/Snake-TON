using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public GameObject Parent;
    public Movement ParentMovement;

    public float BasePartGap = 5f;
    public float PartGap = 5f;
    public float Gap = 0.5f;

    private void Update()
    {
        if (Parent != null)
        {
            Move();
            SetGaps();
            SetScale();
            Rotate();
        }
    }

    private void SetGaps()
    {
        if (Parent.GetComponent<BodyPart>() != null && BasePartGap != Parent.GetComponent<BodyPart>().BasePartGap)
        {
            BasePartGap = Parent.GetComponent<BodyPart>().BasePartGap;
        }

        if (Parent.GetComponent<BodyPart>() != null && PartGap != Parent.GetComponent<BodyPart>().PartGap)
        {
            PartGap = Parent.GetComponent<BodyPart>().PartGap;
        }

        if (Parent.GetComponent<BodyPart>() != null && Gap != Parent.GetComponent<BodyPart>().Gap)
        {
            Gap = Parent.GetComponent<BodyPart>().Gap;
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
        if (GetDistanceToParent() >= Gap)
        {
            float distance = ((Vector2)transform.position - (Vector2)Parent.transform.position).magnitude;
            transform.position = Vector2.Lerp(transform.position, Parent.transform.position, distance / PartGap);
        }
        else if (GetDistanceToParent() < Gap && GetDistanceToParent() >= Gap / 2)
        {
            float distance = ((Vector2)transform.position - (Vector2)Parent.transform.position).magnitude;
            transform.position = Vector2.Lerp(transform.position, Parent.transform.position, distance / (PartGap * 2));
        }
    }

    private float GetDistanceToParent()
    {
        return Vector2.Distance(transform.position, Parent.transform.position);
    }

    private void Rotate()
    {
        Vector2 direction = Parent.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.down, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), ParentMovement.RotationSpeed / 1 * Time.deltaTime);
    }
}

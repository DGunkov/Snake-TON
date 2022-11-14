using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public GameObject Parent;
    public Movement ParentMovement;

    [SerializeField] private float _partsGap = 2f;


    private void Update()
    {
        Rotate();
        transform.position = Vector3.Lerp(transform.position, Parent.transform.position, Time.deltaTime * ParentMovement.Speed * _partsGap);
        transform.localScale = Parent.transform.localScale;
    }

    private void Rotate()
    {
        Vector2 direction = Parent.transform.position - transform.position;
        float angle = Vector2.SignedAngle(Vector2.down, direction);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), ParentMovement.RotationSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public GameObject Parent;
    public Movement Movement;
    [SerializeField] private float _partsGap = 2f;

    private void Update()
    {
        Parent.transform.LookAt(Parent.transform);
        transform.position = Vector3.Lerp(transform.position, Parent.transform.position, Time.deltaTime * Movement.Speed * _partsGap);
        transform.localScale = Parent.transform.localScale;
    }
}

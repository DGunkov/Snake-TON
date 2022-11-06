using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public GameObject Parent;
    [SerializeField] private Movement _movement;

    [SerializeField] private float _partsGap = 2f;

    private Grow _grow;

    private void Start()
    {
        _grow = Parent.GetComponent<Grow>();
        _grow.Parts.Add(this.gameObject);
        if (Parent.GetComponent<Movement>() != null)
        {
            _movement = Parent.GetComponent<Movement>();
        }
        else if (Parent.GetComponent<BodyPart>() != null)
        {
            _movement = Parent.GetComponent<BodyPart>().Movement;
        }
    }

    private void Update()
    {
        Rotate();
        transform.position = Vector3.Lerp(transform.position, Parent.transform.position, Time.deltaTime * _movement.Speed * _partsGap);
        transform.localScale = Parent.transform.localScale;
    }

    private void Rotate()
    {
        if (Parent != null)
        {
            Vector2 direction = Parent.transform.position - transform.position;
            float angle = Vector2.SignedAngle(Vector2.down, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), _movement.RotationSpeed * Time.deltaTime);
        }
        else
        {
            Parent = _grow.Parts[_grow.Parts.Count - 2];
        }
    }
}

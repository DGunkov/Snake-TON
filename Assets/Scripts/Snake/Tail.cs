using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tail : MonoBehaviour
{
    public GameObject Parent;
    [SerializeField] private Movement _movement;

    private Grow _grow;

    private void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            foreach (PlayerInput player in FindObjectsOfType<PlayerInput>())
            {
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    _grow = player.GetComponent<Grow>();
                }
            }

            if (Parent.GetComponent<Movement>() != null)
            {
                _movement = Parent.GetComponent<Movement>();
            }
            else if (Parent.GetComponent<BodyPart>() != null)
            {
                _movement = Parent.GetComponent<BodyPart>().ParentMovement;
            }
        }
        else
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        Rotate();
        Move();
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
            if (_grow.Parts[_grow.Parts.Count - 2] != null)
            {
                Parent = _grow.Parts[_grow.Parts.Count - 2];
                Rotate();
            }
        }
    }

    private void Move()
    {
        BodyPart bodyPart = Parent.GetComponent<BodyPart>();
        if (bodyPart != null)
        {
            if (Vector2.Distance(transform.position, Parent.transform.position) >= bodyPart.Gap)
            {
                float distance = ((Vector2)transform.position - (Vector2)Parent.transform.position).magnitude;
                transform.position = Vector2.Lerp(transform.position, Parent.transform.position, distance / bodyPart.PartGap);
            }
            else if (Vector2.Distance(transform.position, Parent.transform.position) < bodyPart.Gap)
            {
                float distance = ((Vector2)transform.position - (Vector2)Parent.transform.position).magnitude;
                transform.position = Vector2.Lerp(transform.position, Parent.transform.position, distance / (bodyPart.PartGap * 2));
            }
        }
    }

    public void SetScale()
    {
        if (Parent != null)
        {
            transform.localScale = Parent.transform.localScale;
        }
    }
}

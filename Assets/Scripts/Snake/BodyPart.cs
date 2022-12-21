using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BodyPart : MonoBehaviour
{
    public GameObject Parent;
    public Movement ParentMovement;

    private void Awake()
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (Parent != null)
        {
            Vector2 direction = Parent.transform.position - transform.position;
            float angle = Vector2.SignedAngle(Vector2.down, direction);
            Vector3 targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), ParentMovement.RotationSpeed * Time.deltaTime);

            if (transform.localScale != Parent.transform.localScale)
            {
                transform.localScale = Parent.transform.localScale;
            }
        }
    }
}

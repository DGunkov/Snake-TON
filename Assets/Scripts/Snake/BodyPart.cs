using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BodyPart : MonoBehaviour
{
    public GameObject Parent;
    public Movement ParentMovement;
    private bool _spawn = true;
    private float time_to_spawn_scale = 1;
    private float _correct_angle;

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
            if(angle > 15)
            {
                _correct_angle = 0.85f;
            }
            else
            {
                _correct_angle = 1;
            }
            Vector3 targetRotation = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), ParentMovement.RotationSpeed * Time.deltaTime * _correct_angle);
            if(time_to_spawn_scale > 0)
            {
                time_to_spawn_scale -= Time.deltaTime;
                transform.localScale = Vector3.Lerp(Parent.transform.localScale, new Vector3(0.01f, 0.01f, 0), time_to_spawn_scale);
            }
            else
            {
                if (transform.localScale != Parent.transform.localScale)
                {
                    transform.localScale = Parent.transform.localScale;
                }
            }
            
        }
    }
}

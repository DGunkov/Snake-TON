using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Navigation : MonoBehaviour
{
    void Start()
    {
        if(!transform.parent.GetComponent<PhotonView>().IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        transform.up = -(new Vector3(0, 0, 0) - transform.position);
    }
}

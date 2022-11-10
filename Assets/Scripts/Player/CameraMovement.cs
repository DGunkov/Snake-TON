using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraMovement : MonoBehaviour
{
    public Transform Snake;
    private PhotonView _view;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        if (!_view.IsMine)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Snake != null)
        {
            transform.position = new Vector3(Snake.position.x, Snake.position.y, -10);
        }
    }
}

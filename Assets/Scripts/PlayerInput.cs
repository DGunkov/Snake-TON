using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{
    public bool KeyboardInput = false;

    [SerializeField] private float _cameraMultyplier = 0.01f;

    private Movement _movement;

    private void Start()
    {
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        RotationInput();
    }

    private void RotationInput()
    {
#if UNITY_STANDALONE
        if (KeyboardInput)
        {
            float direction = -Input.GetAxisRaw("Horizontal");
            _movement.Rotate(direction);
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            _movement.Rotate(direction);
        }
#endif
    }

    public void CameraZoom()
    {
        StartCoroutine(CameraRemote());
    }

    private IEnumerator CameraRemote()
    {
        for (int i = 0; i < 5; i++)
        {
            Camera.main.orthographicSize += _cameraMultyplier;
            yield return new WaitForSeconds(0.35f);
        }
    }

}

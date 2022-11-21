using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Food : MonoBehaviour
{
    public float Satiety;
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Color _color;
    private GameObject _player;

    private void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = _sprite;
        GetComponentInChildren<SpriteRenderer>().color = _color;
        _animator = GetComponent<Animator>();
        _animator.speed = Random.Range(0.5f, 2f);
        transform.localScale = Vector3.one * Satiety * 3;
        foreach (PlayerInput player in FindObjectsOfType<PlayerInput>())
        {
            if (player.gameObject.GetComponent<PhotonView>().IsMine)
            {
                _player = player.gameObject;
            }
        }
    }

    private void Update()
    {
        if (Vector2.Distance(_player.transform.position, this.transform.position) < DataHolder.RenderDistance)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
        else
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }
}

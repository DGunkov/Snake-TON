using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Food : MonoBehaviour
{
    [SerializeField] internal float Satiety;
    [SerializeField] private Animator _animator;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Color _color;
    private GameObject _player;
    private SpriteRenderer _sprite_renderer;

    private void Awake()
    {
        _sprite_renderer = GetComponent<SpriteRenderer>();
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
        if(_player != null)
        {
            if (Vector2.Distance(_player.transform.position, this.transform.position) < DataHolder.RenderDistance)
            {
                _sprite_renderer.enabled = true;
            }
            else
            {
                _sprite_renderer.enabled = false;
            }
        }        
    }
}

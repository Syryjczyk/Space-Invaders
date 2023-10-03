using System;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private float animationTime;
    [SerializeField] private int score;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;

    public Action<Invader> Killed;
    public int Score => score;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _animationFrame = 0;
        InvokeRepeating(nameof(AnimateInvader), animationTime, animationTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Killed?.Invoke(this);
        }
    }

    private void AnimateInvader()
    {
        _animationFrame++;

        if (_animationFrame >= animationSprites.Length)
        {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = animationSprites[_animationFrame];
    }
}

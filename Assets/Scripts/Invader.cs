using System;
using UnityEngine;

public class Invader : MonoBehaviour
{
    [SerializeField] private Sprite[] animationSprites;
    [SerializeField] private float animationTime;
    [SerializeField] private int score;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    public Action<Invader> Killed;
    public int Score => score;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        animationFrame = 0;
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
        animationFrame++;

        if (animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
    }
}

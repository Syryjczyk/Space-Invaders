using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;

    private BoxCollider2D _boxCollider;

    public Action<Projectile> Destroyed;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void OnDestroy()
    {
        if (Destroyed != null)
        {
            Destroyed.Invoke(this);
        }
    }

    private void CheckCollision(Collider2D collision)
    {
        Bunker bunker = collision.gameObject.GetComponent<Bunker>();

        if (bunker == null || bunker.CheckCollision(_boxCollider, transform.position))
        {
            Destroy(gameObject);
        }
    }
}

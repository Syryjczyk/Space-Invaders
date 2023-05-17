using System;
using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float cycleTime;
    [SerializeField] private int score;
    [SerializeField] private float padding;

    private Vector3 leftDestination;
    private Vector3 rightDestination;
    private int direction = -1;
    private bool spawned;

    public Action<MysteryShip> Killed;
    public int Score => score;

    private void Start()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        Vector3 left = transform.position;
        left.x = leftEdge.x - padding;
        leftDestination = left;

        Vector3 right = transform.position;
        right.x = rightEdge.x + padding;
        rightDestination = right;

        transform.position = leftDestination;
        Despawn();
    }

    private void Update()
    {
        if (!spawned)
        {
            return;
        }

        if (direction == 1)
        {
            MoveRight();
        }
        else
        {
            MoveLeft();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();

            if (Killed != null)
            {
                Killed.Invoke(this);
            }
        }
    }

    private void MoveRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x >= rightDestination.x)
        {
            Despawn();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= leftDestination.x)
        {
            Despawn();
        }
    }

    private void Spawn()
    {
        direction *= -1;

        if (direction == 1)
        {
            transform.position = leftDestination;
        }
        else
        {
            transform.position = rightDestination;
        }

        spawned = true;
    }

    private void Despawn()
    {
        spawned = false;

        if (direction == 1)
        {
            transform.position = rightDestination;
        }
        else
        {
            transform.position = leftDestination;
        }

        Invoke(nameof(Spawn), cycleTime);
    }
}

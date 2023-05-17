using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private float timeRate;
    [Header("Position")]
    [SerializeField] private float xPosition;
    [SerializeField] private float yPosition;

    private SpriteRenderer spriteRenderer;
 
    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Invoke(nameof(RandomizeInvader), timeRate);
    }

    private void RandomizeInvader()
    {
        RandomizeTransform();
        RandomizeColor();
    }

    private void RandomizeTransform()
    {
        float xAxis = Random.Range(-xPosition, xPosition);
        float yAxis = Random.Range(-yPosition, yPosition);

        transform.position = new Vector3(xAxis, yAxis, 0);

        float rotation = Random.Range(0, 360);

        transform.Rotate(new Vector3(0, 0, rotation));
    }

    private void RandomizeColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        spriteRenderer.color = new Color(r, g, b);
    }
}

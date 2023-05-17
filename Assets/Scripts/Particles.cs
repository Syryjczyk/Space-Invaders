using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    private Player player;
    private Invaders invaders;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
    }

    private void OnEnable()
    {
        player.Killed += ShowParticles;
        invaders.Killed += ShowParticles;
    }

    private void OnDisable()
    {
        player.Killed -= ShowParticles;
        invaders.Killed -= ShowParticles;
    }

    private void ShowParticles(Transform objectTransform)
    {
        transform.position = objectTransform.position;
        particles.Play();
    }

    private void ShowParticles(Invader invader)
    {
        transform.position = invader.transform.position;
        particles.Play();
    }
}

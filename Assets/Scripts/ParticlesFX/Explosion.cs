using UnityEngine;

public class Explosion : MonoBehaviour
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
        player.Killed += ExplosionParticles;
        invaders.Killed += ExplosionParticles;
    }

    private void OnDisable()
    {
        player.Killed -= ExplosionParticles;
        invaders.Killed -= ExplosionParticles;
    }

    private void ExplosionParticles(Transform objectTransform)
    {
        transform.position = objectTransform.position;
        particles.Play();
    }

    private void ExplosionParticles(Invader invader)
    {
        transform.position = invader.transform.position;
        particles.Play();
    }
}

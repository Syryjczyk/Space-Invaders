using UnityEngine;

public class Smoke: MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float offset;

    private Player _player;
    private Invaders _invaders;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _invaders = FindObjectOfType<Invaders>();
    }

    private void OnEnable()
    {
        _player.Smoke += SmokeParticles;
        _invaders.Smoke += SmokeParticles;
    }

    private void OnDisable()
    {
        _player.Smoke -= SmokeParticles;
        _invaders.Smoke -= SmokeParticles;
    }

    private void SmokeParticles(Invader invader)
    {
        transform.position = invader.transform.position;
        particles.Play();
    }

    private void SmokeParticles(Transform objectTransform)
    {
        transform.position = new Vector2(objectTransform.position.x, objectTransform.position.y + offset);
        particles.Play();
    }
}

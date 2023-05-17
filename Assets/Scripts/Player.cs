using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform playerObject;
    [SerializeField] private Projectile laser;
    [SerializeField] private float movementSpeed;
    [Header("Sounds")]
    [SerializeField] private AudioSource playerShootSFX;
    [SerializeField] private AudioSource explosionSFX;

    private GameControls gameControls;
    private Bounds screenBounds;
    private Vector2 direction;
    private bool readyToShoot = true;

    public Action<Transform> Killed;

    private void Awake()
    {
        gameControls = new GameControls();
    }

    private void OnEnable()
    {
        gameControls.PlayerActions.Enable();
        gameControls.PlayerActions.Movement.performed += MoveAction;
        gameControls.PlayerActions.Shooting.performed += ShootLaser;
    }

    private void Start()
    {
        float cameraDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraDistance));
        screenBounds = new Bounds((topRight + bottomLeft) / 2, topRight - bottomLeft);
    }

    private void Update()
    {
        playerObject.Translate(direction.x * movementSpeed * Time.deltaTime, 0f, 0f);

        Vector3 clampedPosition = playerObject.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, screenBounds.min.x + 0.12f, screenBounds.max.x - 0.12f);
        playerObject.position = clampedPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader") || collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            explosionSFX.Play();

            if (Killed != null)
            {
                Killed.Invoke(transform);
            }
        }
    }

    private void ShootLaser(InputAction.CallbackContext context)
    {
        if (readyToShoot)
        {
            Projectile projectile = Instantiate(laser, transform.position, Quaternion.identity);
            projectile.Destroyed += ShootReadyCheck;
            readyToShoot = false;
            playerShootSFX.Play();
        }
    }

    private void MoveAction(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    private void ShootReadyCheck(Projectile laser)
    {
        readyToShoot = true;
    }
}

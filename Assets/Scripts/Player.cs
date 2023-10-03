using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform playerObject;
    [SerializeField] private Projectile laser;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Sprite[] cannonSprites;
    [Header("Sounds")]
    [SerializeField] private AudioSource playerShootSFX;
    [SerializeField] private AudioSource explosionSFX;

    private GameControls _gameControls;
    private SpriteRenderer _cannonSprite;
    private Bounds _screenBounds;
    private Vector2 _direction;
    private bool _readyToShoot = true;

    public Action<Transform> Killed;
    public Action<Transform> Smoke;

    private void Awake()
    {
        _gameControls = new GameControls();
    }

    private void OnEnable()
    {
        _gameControls.PlayerActions.Enable();
        _gameControls.PlayerActions.Movement.performed += MoveAction;
        _gameControls.PlayerActions.Shooting.performed += ShootLaser;
    }

    private void OnDisable()
    {
        _gameControls.PlayerActions.Movement.performed -= MoveAction;
        _gameControls.PlayerActions.Shooting.performed -= ShootLaser;
        _gameControls.PlayerActions.Disable();
    }

    private void Start()
    {
        _cannonSprite = GetComponent<SpriteRenderer>();
        float cameraDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraDistance));
        _screenBounds = new Bounds((topRight + bottomLeft) / 2, topRight - bottomLeft);
    }

    private void Update()
    {
        playerObject.Translate(_direction.x * movementSpeed * Time.deltaTime, 0f, 0f);

        Vector3 clampedPosition = playerObject.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, _screenBounds.min.x + 0.12f, _screenBounds.max.x - 0.12f);
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
        if (_readyToShoot)
        {
            if (Smoke != null)
            {
                Smoke.Invoke(transform);
            }

            StartCoroutine(nameof(ShootAnimation));
            VibrationHandler.MediumVibration();
            Projectile projectile = Instantiate(laser, transform.position, Quaternion.identity);
            projectile.Destroyed += ShootReadyCheck;
            _readyToShoot = false;
            playerShootSFX.Play();
        }
    }

    private void MoveAction(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }

    private void ShootReadyCheck(Projectile laser)
    {
        _readyToShoot = true;
    }

    private IEnumerator ShootAnimation()
    {
        _cannonSprite.sprite = cannonSprites[1];

        yield return new WaitForSeconds(0.25f);

        _cannonSprite.sprite = cannonSprites[0];
    }
}

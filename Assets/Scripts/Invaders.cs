using UnityEngine;

public class Invaders : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float spacing;
    [SerializeField] private float padding;
    [SerializeField] private float downDistance;
    [SerializeField] private float shootRate;
    [SerializeField] private AnimationCurve speed;
    [SerializeField] private Projectile missile;
    [SerializeField] private Invader[] invadersRowRepresentation;
    [Header("Sounds")]
    [SerializeField] private AudioSource invaderShootSFX;
    [SerializeField] private AudioSource explosionSFX;

    private Vector3 direction = Vector3.right;
    private Vector3 initialPosition;
    private int killedAmount;
    private int totalAmount => rows * columns;
    private int aliveAmount => totalAmount - killedAmount;
    private float percentedKilled => (float)killedAmount / (float)totalAmount;

    public System.Action<Invader> Killed;
    public int KilledAmount => killedAmount;
    public int TotalAmount => totalAmount;

    private void OnEnable()
    {
        initialPosition = transform.position;

        for (int row = 0; row < rows; row++)
        {
            float width = spacing * (columns - 1);
            float height = spacing * (rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * spacing), 0);

            for (int col = 0; col < columns; col++)
            {
                Invader invader = Instantiate(invadersRowRepresentation[row], transform);
                invader.Killed += OnInvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * spacing;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(ShootMissile), shootRate, shootRate);
    }

    private void Update()
    {
        transform.position += direction * speed.Evaluate(percentedKilled) * Time.deltaTime;

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
            Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);

            if (direction == Vector3.right && invader.position.x > (rightEdge.x - padding))
            {
                AdvanceRow();
            }
            else if (direction == Vector3.left && invader.position.x < (leftEdge.x + padding))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        direction.x *= -1.0f;
        Vector3 position = transform.position;
        position.y -= downDistance;
        transform.position = position;
    }

    private void OnInvaderKilled(Invader invader)
    {
        explosionSFX.Play();
        invader.gameObject.SetActive(false);
        killedAmount++;
        Killed(invader);
    }

    private void ShootMissile()
    {
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1.0f / aliveAmount))
            {
                Instantiate(missile, invader.transform.position, Quaternion.identity);
                invaderShootSFX.Play();
                break;
            }
        }
    }

    public void ResetInvaders()
    {
        //killedAmount = 0;
        //direction = Vector3.right;
        //transform.position = initialPosition;

        //foreach (Transform invader in transform)
        //{
        //    invader.gameObject.SetActive(true);
        //}

        direction = Vector3.right;
        transform.position = initialPosition;
    }
}

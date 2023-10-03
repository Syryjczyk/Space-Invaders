using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float padding;
    [SerializeField] private float spawnFrequency;
    [SerializeField] private int invadersAmount;
    [SerializeField] private Invader[] invadersArray;

    private Vector3 _spawningXPosition;

    private void OnEnable()
    {
        invadersAmount = 0;
    }

    private void Start()
    {
        InvokeRepeating(nameof(InvaderProcedure), spawnFrequency, spawnFrequency);
    }

    private void InvaderProcedure()
    {
        SetXPosition();
        SpawnInvader();
    }

    private void SetXPosition()
    {
        Vector3 rightEdgeVector = Camera.main.ViewportToWorldPoint(Vector3.right);
        Vector3 leftEdgeVector = Camera.main.ViewportToWorldPoint(Vector3.zero);

        float rightEdge = rightEdgeVector.x - padding;
        float leftEdge = leftEdgeVector.x + padding;
        float randomizeX = Random.Range(leftEdge, rightEdge);

        transform.position = new Vector3(randomizeX, transform.position.y, transform.position.z);
    }

    private void SpawnInvader()
    {
        int randomIndex = Random.Range(0, invadersArray.Length);
        Invader invader = invadersArray[randomIndex];

        Instantiate(invader);
    }
}

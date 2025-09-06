using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab; // assign in Inspector (prefab)
    [SerializeField] private Transform spawnPoint;  // assign in Inspector (empty transform)

    private GameObject currentBall;

    void Start()
    {
        var existing = GameObject.FindWithTag("Ball");
        if (existing != null)
        {
            currentBall = existing;

            var b = currentBall.GetComponent<Ball>();
            if (b != null) b.SetReady(true);
            return;
        }

        if (ballPrefab != null)
        {
            Vector2 pos = spawnPoint != null ? (Vector2)spawnPoint.position : (Vector2)transform.position;
            SpawnBall(pos);
        }
        else
        {
            Debug.LogError("BallSpawner: ballPrefab not assigned in Inspector.");
        }
    }

    private void SpawnBall(Vector2 position)
    {
        currentBall = Instantiate(ballPrefab, position, Quaternion.identity);
        currentBall.tag = "Ball"; 
        var b = currentBall.GetComponent<Ball>();
        if (b != null) b.SetReady(true);
    }

    public void ReadyBall(Vector2 newPosition)
    {
        if (ballPrefab == null && currentBall == null)
        {
            Debug.LogError("BallSpawner: no ballPrefab and no currentBall.");
            return;
        }

        if (currentBall == null)
        {
            SpawnBall(newPosition);
            return;
        }

        Rigidbody2D rb = currentBall.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector2.zero;

        currentBall.transform.position = newPosition;

        var ballScript = currentBall.GetComponent<Ball>();
        if (ballScript != null) ballScript.SetReady(true);
    }
}

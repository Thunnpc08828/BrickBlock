using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab; // assign in Inspector (prefab)
    [SerializeField] private Transform _spawnPoint;  // assign in Inspector (empty transform)

    [SerializeField] private GameObject _currentBall;

    void Start()
    {
        var existing = GameObject.FindWithTag("Ball");
        if (existing != null)
        {
            _currentBall = existing;

            var b = _currentBall.GetComponent<Ball>();
            if (b != null) b.SetReady(true);
            return;
        }

        if (_ballPrefab != null)
        {
            Vector2 pos = _spawnPoint != null ? (Vector2)_spawnPoint.position : (Vector2)transform.position;
            SpawnBall(pos);
        }
        else
        {
            Debug.LogError("BallSpawner: ballPrefab not assigned in Inspector.");
        }
    }

    private void SpawnBall(Vector2 position)
    {
        _currentBall = Instantiate(_ballPrefab, position, Quaternion.identity);
        _currentBall.tag = "Ball"; 
        var b = _currentBall.GetComponent<Ball>();
        if (b != null) b.SetReady(true);
    }

    public void ReadyBall(Vector2 newPosition)
    {
        if (_ballPrefab == null && _currentBall == null)
        {
            Debug.LogError("BallSpawner: no ballPrefab and no currentBall.");
            return;
        }

        if (_currentBall == null)
        {
            SpawnBall(newPosition);
            return;
        }

        Rigidbody2D rb = _currentBall.GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector2.zero;

        _currentBall.transform.position = newPosition;

        var ballScript = _currentBall.GetComponent<Ball>();
        if (ballScript != null) ballScript.SetReady(true);
    }
}

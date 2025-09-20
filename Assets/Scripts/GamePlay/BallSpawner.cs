using UnityEngine;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab; // assign in Inspector (prefab)
    [SerializeField] private Transform _spawnPoint;  // assign in Inspector (empty transform)

    [SerializeField] private List<GameObject> _balls = new List<GameObject>(); 

    private int _ballsReturned = 0; 
    private Vector2 _lastBallPos;   

    void Start()
    {
        var existing = GameObject.FindWithTag("Ball");
        if (existing != null)
        {
            _balls.Add(existing);

            var b = existing.GetComponent<Ball>();
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
        GameObject newBall = Instantiate(_ballPrefab, position, Quaternion.identity);
        newBall.tag = "Ball";
        var b = newBall.GetComponent<Ball>();
        if (b != null) b.SetReady(true);

        _balls.Add(newBall);
    }

    public void ReadyBall(Vector2 newPosition)
    {
        if (_ballPrefab == null && _balls.Count == 0)
        {
            Debug.LogError("BallSpawner: no ballPrefab and no balls.");
            return;
        }

        foreach (var ball in _balls)
        {
            if (ball == null) continue;

            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;

            ball.transform.position = newPosition;

            var ballScript = ball.GetComponent<Ball>();
            if (ballScript != null) ballScript.SetReady(true);
        }

        _ballsReturned = 0; 
    }

    public void AddBall()
    {
        Vector2 pos = _spawnPoint != null ? (Vector2)_spawnPoint.position : (Vector2)transform.position;
        SpawnBall(pos);
        Debug.Log("AddBall: now have " + _balls.Count);
    }

    public void SpawnAndShootBall(Vector2 position, Vector2 direction, int count = 1)
    {
        float baseSpeed = 10f;
        float offset = 1.5f; 

        for (int i = 0; i < count; i++)
        {
            GameObject newBall = Instantiate(_ballPrefab, position, Quaternion.identity);
            newBall.tag = "Ball";

            var b = newBall.GetComponent<Ball>();
            if (b != null)
            {
                b.SetReady(false);
                Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    float speed = baseSpeed + (i - count / 2f) * offset;
                    rb.velocity = direction.normalized * speed;
                }
            }

            _balls.Add(newBall);
        }
    }

    public void OnBallReturned(GameObject ball)
    {
        _ballsReturned++;
        _lastBallPos = ball.transform.position;

        if (_ballsReturned >= _balls.Count)
        {          
            ReadyBall(_lastBallPos);
        }
    }
}

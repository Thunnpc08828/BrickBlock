using UnityEngine;

public class Below : MonoBehaviour
{
    private BallSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<BallSpawner>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            spawner.ReadyBall();
        }
    }
}

using UnityEngine;

public class WallBottom : MonoBehaviour
{
    private BallSpawner spawner;
    private BlockSpawner blockSpawner;

    void Start()
    {
        spawner = FindObjectOfType<BallSpawner>();
        blockSpawner = FindObjectOfType<BlockSpawner>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // bóng gi? nguyên v? trí rõi
            Vector2 newPos = collision.transform.position;
            spawner.ReadyBall(newPos);

            // n?u không c?n block nào th? spawn l?i
            if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
            {
                blockSpawner.SpawnBlocks();
            }
        }
    }
}

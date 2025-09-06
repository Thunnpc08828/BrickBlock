using System.Collections;
using UnityEngine;

public class WallBottom : MonoBehaviour
{
    private BallSpawner spawner;
    private BlockSpawner blockSpawner;
    private bool handling = false;

    void Start()
    {
        spawner = FindObjectOfType<BallSpawner>();
        blockSpawner = FindObjectOfType<BlockSpawner>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;
        if (handling) return; 

        handling = true;
        StartCoroutine(ResetHandling());

        Vector2 contactPoint = collision.GetContact(0).point;

        float ballHalf = 0.25f; 
        var ballCol = collision.collider;
        if (ballCol != null) ballHalf = ballCol.bounds.extents.y;

        float bottomHalf = 0.25f;
        var bottomCol = GetComponent<Collider2D>();
        if (bottomCol != null) bottomHalf = bottomCol.bounds.extents.y;

        float gap = 0.02f;

        Vector2 newPos = contactPoint + Vector2.up * (ballHalf + bottomHalf + gap);

        if (spawner != null) spawner.ReadyBall(newPos);

        if (GameObject.FindGameObjectsWithTag("Block").Length == 0 && blockSpawner != null)
        {
            blockSpawner.SpawnBlocks();
        }
    }

    IEnumerator ResetHandling()
    {
        yield return new WaitForSeconds(0.06f);
        handling = false;
    }
}

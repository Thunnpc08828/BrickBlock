using System.Collections;
using UnityEngine;

public class WallBottom : MonoBehaviour
{
    [SerializeField] private BallSpawner _spawner;
    [SerializeField] private LevelManager _blockSpawner;
    [SerializeField] private bool _handling = false;
    void Start()
    {
        _spawner = FindObjectOfType<BallSpawner>();
        _blockSpawner = FindObjectOfType<LevelManager>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;
        if (_handling) return;

        if (!BallSpawner.GameStarted) return;

        _handling = true;
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

        if (_spawner != null) _spawner.ReadyBall(newPos);

        if (_blockSpawner != null)
        {
            _blockSpawner.MoveBlocksDown(0.25f);
        }

        if (GameObject.FindGameObjectsWithTag("Block").Length == 0 && _blockSpawner != null)
        {
            _blockSpawner.SpawnBlock();
        }
    }

    IEnumerator ResetHandling()
    {
        yield return new WaitForSeconds(0.06f);
        _handling = false;
    }
}

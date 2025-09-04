using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public int blockCount = 3;
    public Vector2 spawnAreaMin = new Vector2(-6f, 0f); // góc trái dý?i
    public Vector2 spawnAreaMax = new Vector2(6f, 4f);  // góc ph?i trên
    public float minDistance = 1.2f; // kho?ng cách t?i thi?u gi?a các block

    private void Start()
    {
        SpawnBlocks();
    }

    // Ð?t public ð? có th? g?i t? script khác (vd: Below)
    public void SpawnBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            Vector2 spawnPos;
            int attempts = 0;

            do
            {
                // random v? trí trong vùng spawn
                float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                spawnPos = new Vector2(x, y);
                attempts++;

                // tránh v?ng l?p vô h?n
                if (attempts > 50) break;

            } while (!IsValidPosition(spawnPos));

            Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        }
    }

    private bool IsValidPosition(Vector2 pos)
    {
        // ki?m tra có block nào g?n quá không
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, minDistance);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Block")) return false; // ð? có block khác
        }
        return true;
    }
}

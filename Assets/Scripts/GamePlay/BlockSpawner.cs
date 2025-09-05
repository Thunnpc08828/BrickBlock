using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public int blockCount = 3;
    public Vector2 spawnAreaMin = new Vector2(-6f, 0f); // goc trai dý?i
    public Vector2 spawnAreaMax = new Vector2(6f, 4f);  // goc phai trên
    public float minDistance = 1.2f; // khoang cach giua cac block

    private void Start()
    {
        SpawnBlocks();
    }

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

                if (attempts > 50) break;

            } while (!IsValidPosition(spawnPos));

            Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        }
    }

    private bool IsValidPosition(Vector2 pos)
    {
        // kiem tra co block nao gan qua khong
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, minDistance);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Block")) return false; 
        }
        return true;
    }
}

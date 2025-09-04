using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public int blockCount = 3;
    public Vector2 spawnAreaMin = new Vector2(-6f, 0f); // g�c tr�i d�?i
    public Vector2 spawnAreaMax = new Vector2(6f, 4f);  // g�c ph?i tr�n
    public float minDistance = 1.2f; // kho?ng c�ch t?i thi?u gi?a c�c block

    private void Start()
    {
        SpawnBlocks();
    }

    // �?t public �? c� th? g?i t? script kh�c (vd: Below)
    public void SpawnBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            Vector2 spawnPos;
            int attempts = 0;

            do
            {
                // random v? tr� trong v�ng spawn
                float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                spawnPos = new Vector2(x, y);
                attempts++;

                // tr�nh v?ng l?p v� h?n
                if (attempts > 50) break;

            } while (!IsValidPosition(spawnPos));

            Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        }
    }

    private bool IsValidPosition(Vector2 pos)
    {
        // ki?m tra c� block n�o g?n qu� kh�ng
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, minDistance);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Block")) return false; // �? c� block kh�c
        }
        return true;
    }
}

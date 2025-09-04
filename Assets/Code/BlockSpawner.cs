using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;   // Prefab Block
    public int blockCount = 3;       // s? lý?ng block c?n spawn
    public float minX = -7f;         // gi?i h?n ngang bên trái
    public float maxX = 7f;          // gi?i h?n ngang bên ph?i
    public float minY = 0f;          // gi?i h?n d?c dý?i
    public float maxY = 4f;          // gi?i h?n d?c trên

    void Start()
    {
        SpawnBlocks();
    }

    void SpawnBlocks()
    {
        for (int i = 0; i < blockCount; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector2 spawnPos = new Vector2(randomX, randomY);

            Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        }
    }
}

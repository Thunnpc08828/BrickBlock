using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private int X = 8;                  // number of columns
    [SerializeField] private int Y = 4;                  // number of rows
    [SerializeField] private GameObject blockPrefab;     // assign block prefab in Inspector

    [SerializeField] private float marginTop = 0.5f; // distance from top edge
    [SerializeField] private float marginSide = 0.5f; // distance to 2 sides
    [SerializeField] private float blockAreaHeight = 3f; // block height (top to bottom)

    public void SpawnBlocks()
    {
        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight * Camera.main.aspect;

        // Top limit
        float top = Camera.main.orthographicSize - marginTop;
        // Lower limit of block area 
        float bottom = top - blockAreaHeight;

        for (int y = 0; y < Y; y++)
        {
            float tY = (Y == 1) ? 0.5f : (float)y / (Y - 1);
            float posY = Mathf.Lerp(top, bottom, tY);

            for (int x = 0; x < X; x++)
            {
                float tX = (X == 1) ? 0.5f : (float)x / (X - 1);
                float posX = Mathf.Lerp(
                    -camWidth / 2 + marginSide,
                    camWidth / 2 - marginSide,
                    tX
                );

                Instantiate(blockPrefab, new Vector2(posX, posY), Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        SpawnBlocks();
    }
}

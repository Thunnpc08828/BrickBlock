using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private int _x = 1;
    [SerializeField] private int _y = 1;
    [SerializeField] private GameObject _blockPrefab;

    private int[,] _board;
    private void Start()
    {
        _board = new int[_x, _y];
        SpawnBlocks();

    }

    public void SpawnBlocks()
    {
        var x = _board.GetLength(0);
        var y = _board.GetLength(1);
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                var blokc = Instantiate(_blockPrefab, new Vector2(i, j), Quaternion.identity);
            }
        }
    }
}

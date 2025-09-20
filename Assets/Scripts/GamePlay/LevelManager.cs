using Sirenix.OdinInspector;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _x = 8;  
    [SerializeField] private int _y = 4;
    [SerializeField] private GameObject _blockContainer;
    [SerializeField] private Block _blockPrefab;

    [SerializeField] private BlockData[,] _blockDataArray;
    [SerializeField] private LevelData _levelData;

    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _maxLevel = 7;

    public static LevelManager Instance;

    [Button]
    public void SpawnBlock()
    {
        _blockDataArray = new BlockData[_x, _y];

        for (var i =0;  i< _y; i++)
        {
            for(var j =0; j < _x; j++) 
            {
                var localPos = new Vector2(j, i);
                var block = Instantiate(_blockPrefab, _blockContainer.transform);
                var blockData = new BlockData
                {
                    Type = BlockType.Normal,
                    Health = Random.Range(1, 5),
                    X = i,
                    Y = j,
                };
                block.Setup(blockData);
                block.transform.localPosition = localPos;
                _blockDataArray[j, i] = block.BlockData;
            }
        }
    }

    [Button]
    private void DeleteBlock()
    {
        var childCount = _blockContainer.transform.childCount;

        for (var i = childCount -1; i >= 0; i--)
        {
            var block = _blockContainer.transform.GetChild(i);
            Destroy(block.gameObject);
        }
    }

    [Button]
    private void SaveLevel(int level)
    {
        Debug.Log("Save level: " + level);
        _levelData = new LevelData();
        _levelData.Level = level;
        _levelData.BlockDataArry = _blockDataArray;
        var json  = JsonConvert.SerializeObject(_levelData);

        var path = Application.dataPath + $"/Resources/LevelData/{level}.json";
        File.WriteAllText(path, json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    [Button]
    private void LoadLevel(int level)
    {
        Debug.Log("Load level: " + level);

        DeleteBlock();
        _currentLevel = level;

        var path = Application.dataPath + $"/Resources/LevelData/{level}.json";
        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            return;
        }

        var json = File.ReadAllText(path);
        _levelData = JsonConvert.DeserializeObject<LevelData>(json);

        if (_levelData == null || _levelData.BlockDataArry == null)
        {
            Debug.LogError("Error: level data null!");
            return;
        }

        _blockDataArray = _levelData.BlockDataArry;
        int cols = _blockDataArray.GetLength(0); // X
        int rows = _blockDataArray.GetLength(1); // Y

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var data = _blockDataArray[j, i];
                if (data == null || data.Type == BlockType.Empty)
                    continue;

                var block = Instantiate(_blockPrefab, _blockContainer.transform);
                block.Setup(data);
                block.transform.localPosition = new Vector2(j, i);
                block.name = $"Block_{j}_{i}";
            }
        }

        int totalBlocks = _blockContainer.transform.childCount;
        Debug.Log($"Load level {_currentLevel} total blocks: {totalBlocks}");
    }

    public void OnBlockDestroyed()
    {
        StartCoroutine(CheckBlocksEmpty());
    }

    private IEnumerator CheckBlocksEmpty()
    {
        yield return null; 
        if (_blockContainer.transform.childCount == 0)
        {
            StartCoroutine(GoToNextLevel());
        }
    }

    private IEnumerator GoToNextLevel()
    {
        yield return new WaitForSeconds(1.5f);

        _currentLevel++;
        if (_currentLevel <= _maxLevel)
        {
            LoadLevel(_currentLevel);
        }
    }
}

using Newtonsoft;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _x = 8;
    [SerializeField] private int _y = 4;
    [SerializeField] private GameObject _blockContainer;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private BlockData[,] _blockDataArray;
    [SerializeField] private LevelData _levelData;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _maxLevel = 10;
    [SerializeField] private Block _boomPrefab;

    public static LevelManager Instance;

    private Vector3 _containerStartPos;

    void Start()
    {
        if (_blockContainer != null)
        {
            _blockContainer.transform.position = new Vector3(-3.5f, 4f, 0f);
        }
    }

    [Button]
    public void SpawnBlock()
    {
        if (_blockContainer != null) _blockContainer.transform.position = _containerStartPos;

        _blockDataArray = new BlockData[_x, _y];

        List<BlockType> types = new List<BlockType>();
        for (int i = 0; i < 30; i++) types.Add(BlockType.Normal);
        for (int i = 0; i < 2; i++) types.Add(BlockType.Bomb);

        System.Random rnd = new System.Random();
        for (int i = types.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            (types[i], types[j]) = (types[j], types[i]);
        }

        int index = 0;
        for (var i = 0; i < _y; i++)
        {
            for (var j = 0; j < _x; j++)
            {
                if (index >= types.Count) return;
                BlockType t = types[index++];
                Block prefabToUse = _blockPrefab;
                if (t == BlockType.Bomb) prefabToUse = _boomPrefab;

                var localPos = new Vector2(j, i);
                var block = Instantiate(prefabToUse, _blockContainer.transform);
                var blockData = new BlockData
                {
                    Type = t,
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
        for (var i = childCount - 1; i >= 0; i--)
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

        var json = JsonConvert.SerializeObject(_levelData);
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
                if (data == null || data.Type == BlockType.Empty) continue;

                Block prefabToUse = _blockPrefab;
                if (data.Type == BlockType.Bomb) prefabToUse = _boomPrefab;

                var block = Instantiate(prefabToUse, _blockContainer.transform);
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
        if (_currentLevel >= _maxLevel)
        {
            _currentLevel = 1;
        }
        else
        {
            _currentLevel++;
        }
        LoadLevel(_currentLevel);
    }
    public void MoveBlocksDown(float step = 1f)
    {
        if (_blockContainer == null) return;

        _blockContainer.transform.position += Vector3.down * step;

        foreach (Transform child in _blockContainer.transform)
        {
            var block = child.GetComponent<Block>();
            if (block != null && block.BlockData != null)
            {
                block.BlockData.Y -= step; 
            }
        }
    }
    public void Awake()
    {
        if (_blockContainer != null) _containerStartPos = _blockContainer.transform.position;
    }

    public void ResetContainerPosition()
    {
        if (_blockContainer != null) _blockContainer.transform.position = _containerStartPos;
    }

    public void SpawnNextLevel()
    {
        DeleteBlock();
        ResetContainerPosition(); 
        SpawnBlock();
    }
}

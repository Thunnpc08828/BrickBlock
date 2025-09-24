using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
public class Block : MonoBehaviour
{
    [SerializeField] private TextMeshPro _tmpHealth;
    [SerializeField] private LevelManager _levelManager;

    public BlockData BlockData;

    [SerializeField] private bool _isDestroyed = false;

    public void Setup(BlockData blockData)
    {
        BlockData = blockData;
        _tmpHealth.text = BlockData.Health.ToString();

        _levelManager = FindObjectOfType<LevelManager>();
        if (BlockData.Type == BlockType.Bomb)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                var bombSprite = Resources.Load<Sprite>("Sprites/boom3x3");
                if (bombSprite != null) sr.sprite = bombSprite;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TakeDamage(1, collision);
        }
    }

    public void TakeDamage(int amount, Collision2D collision = null)
    {
        if (_isDestroyed) return; 

        BlockData.Health -= amount;
        _tmpHealth.text = BlockData.Health.ToString();

        if (BlockData.Health <= 0)
        {
            _isDestroyed = true; 

            if (BlockData.Type == BlockType.Bomb)
            {
                Explode();
            }

            if (_levelManager != null)
            {
                _levelManager.OnBlockDestroyed();
            }

            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (_levelManager == null) return;

        int bx = (int)BlockData.X;
        int by = (int)BlockData.Y;

        var allBlocks = GameObject.FindGameObjectsWithTag("Block");

        for (int i = 0; i < allBlocks.Length; i++)
        {
            var block = allBlocks[i].GetComponent<Block>();
            if (block != null)
            {
                int x = (int)block.BlockData.X;
                int y = (int)block.BlockData.Y;

                if (x == bx || y == by)
                {
                    if (_levelManager != null)
                    {
                        _levelManager.OnBlockDestroyed();
                    }
                    Destroy(block.gameObject);
                }
            }
        }
    }

    [Button]
    private void UpdateHealth()
    {
        _tmpHealth.text = BlockData.Health.ToString();
    }
}

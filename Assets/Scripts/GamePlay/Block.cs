using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
public class Block : MonoBehaviour
{
    [SerializeField] private TextMeshPro _tmpHealth;
    [SerializeField] private LevelManager _levelManager;
    public BlockData BlockData;

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
            TakeDamage(1); 
        }
    }

    void TakeDamage(int amount)
    {
        BlockData.Health -= amount;
        _tmpHealth.text = BlockData.Health.ToString();

        if (BlockData.Health <= 0)
        {
            if (BlockData.Type == BlockType.Bomb)
            {
                Explode();
            }

            Destroy(gameObject);
        }
    }

    void Explode()
    {
        if (_levelManager == null) return;

        int bx = (int)BlockData.X;
        int by = (int)BlockData.Y;

        var allBlocks = GameObject.FindGameObjectsWithTag("Block");

        foreach (var b in allBlocks)
        {
            var block = b.GetComponent<Block>();
            if (block != null)
            {
                int x = (int)block.BlockData.X;
                int y = (int)block.BlockData.Y;

                if (x == bx || y == by)
                {
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

using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
public class Block : MonoBehaviour
{
    [SerializeField] private TextMeshPro _tmpHealth;
    public BlockData BlockData;

    public void Setup(BlockData blockData)
    {
        BlockData = blockData;
        _tmpHealth.text = BlockData.Health.ToString();
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
            Destroy(gameObject);
        }
    }

    [Button]
    private void UpdateHealth()
    {
        _tmpHealth.text = BlockData.Health.ToString();
    }
}

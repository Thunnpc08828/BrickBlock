using UnityEngine;
using TMPro; 
public class Block : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] private TextMeshPro _tmpHealth;

    void Start()
    {
        _tmpHealth.text = _health.ToString();
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
        _health -= amount;
        _tmpHealth.text = _health.ToString();

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

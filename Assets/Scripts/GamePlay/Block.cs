using UnityEngine;
using TMPro; // D�ng TextMeshPro

public class Block : MonoBehaviour
{
    public int health = 3; // M�u kh?i t?o
    private TextMeshProUGUI healthText;

    void Start()
    {
        // L?y text con b�n trong block
        healthText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateHealthText();
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
        health -= amount;
        UpdateHealthText();

        if (health <= 0)
        {
            Destroy(gameObject); // Khi mau ve 0 block bien mat
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }
}

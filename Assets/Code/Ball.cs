using UnityEngine;

public class Ball : MonoBehaviour
{
    public float constantSpeed = 10f; // t?c �? c? �?nh

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Lu�n gi? v?n t?c c? �?nh
        rb.velocity = constantSpeed * (rb.velocity.normalized);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Destroy(collision.gameObject);
        }
    }
}

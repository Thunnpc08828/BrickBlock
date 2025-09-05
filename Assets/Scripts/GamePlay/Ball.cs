using UnityEngine;

public class Ball : MonoBehaviour
{
    public float constantSpeed = 10f; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = constantSpeed * (rb.velocity.normalized);
    }
}

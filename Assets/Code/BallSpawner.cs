using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    public float ballSpeed = 10f;

    private Vector2 shootDirection;
    private bool isAiming = false;
    private bool isReady = true;

    void Update()
    {
        if (!isReady) return;

        // Nh?n chu?t �? ng?m
        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
        }

        // Trong khi gi? chu?t -> x�c �?nh h�?ng
        if (isAiming)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - ball.transform.position); // ? h�?ng t? b�ng hi?n t?i
            direction.Normalize();

            shootDirection = direction;
            Debug.DrawRay(ball.transform.position, shootDirection * 5, Color.green);
        }

        // Th? chu?t -> b?n
        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            isAiming = false;
            ShootBall();
        }
    }

    void ShootBall()
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * ballSpeed;
        isReady = false;
    }

    // G?i khi b�ng ch?m "below"
    public void ReadyBall()
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero; // d?ng l?i t?i ch?
        isReady = true;             // cho ph�p b?n ti?p
    }
}

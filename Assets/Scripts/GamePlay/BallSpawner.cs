using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    public float ballSpeed = 10f;
    public LineRenderer aimLine;      // LineRenderer ð? v? tia ng?m

    private Vector2 shootDirection;
    private bool isAiming = false;
    private bool isReady = true;

    void Update()
    {
        if (!isReady) return;

        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true; //trang thai ngam
            aimLine.enabled = true; // hien thi duong ngam
        }

        if (isAiming)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - ball.transform.position);
            direction.Normalize();
            shootDirection = direction;

            float offset = 0.6f; // khoang cach ðuong ngam tach khoi bóng
            float lineLength = 5f; // chieu dai ðuong ngam

            Vector2 startPos = (Vector2)ball.transform.position + direction * offset;
            Vector2 endPos = startPos + direction * lineLength;

            aimLine.SetPosition(0, startPos);
            aimLine.SetPosition(1, endPos);
        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            isAiming = false;
            aimLine.enabled = false; 
            ShootBall();
        }
    }

    void ShootBall()
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * ballSpeed;
        isReady = false;
    }

    public void ReadyBall(Vector2 newPosition)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        ball.transform.position = newPosition;

        isReady = true;
    }
}

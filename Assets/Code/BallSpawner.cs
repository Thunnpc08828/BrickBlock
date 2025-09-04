using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    public float ballSpeed = 10f;
    public LineRenderer aimLine;      // LineRenderer �? v? tia ng?m

    private Vector2 shootDirection;
    private bool isAiming = false;
    private bool isReady = true;

    void Update()
    {
        if (!isReady) return;

        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            aimLine.enabled = true; // b?t tia ng?m
        }

        if (isAiming)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - ball.transform.position);
            direction.Normalize();
            shootDirection = direction;

            float offset = 0.6f; // kho?ng c�ch ��?ng ng?m t�ch kh?i b�ng
            float lineLength = 5f; // chi?u d�i ��?ng ng?m

            Vector2 startPos = (Vector2)ball.transform.position + direction * offset;
            Vector2 endPos = startPos + direction * lineLength;

            aimLine.SetPosition(0, startPos);
            aimLine.SetPosition(1, endPos);
        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            isAiming = false;
            aimLine.enabled = false; // t?t tia ng?m
            ShootBall();
        }
    }

    void ShootBall()
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * ballSpeed;
        isReady = false;
    }

    // Gi? b�ng t?i v? tr� r�i, kh�ng �?ng t?i spawner
    public void ReadyBall(Vector2 newPosition)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

        // �?t b�ng v? ��ng ch? n� r�i
        ball.transform.position = newPosition;

        isReady = true;
    }
}

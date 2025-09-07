using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _constantSpeed = 10f; // keep speed constant
    [SerializeField] private float _ballSpeed = 10f;     // shoot speed
    [SerializeField] private LineRenderer _aimLine;      // assign in prefab/inspector

    [SerializeField] private Rigidbody2D rb;
    private Vector2 shootDirection;
    private bool isAiming = false;
    private bool isReady = true;


    void Start()
    {

    }

    void Update()
    {
        if (!isReady) return;

        if (Input.GetMouseButtonDown(0))
        {
            isAiming = true;
            if (_aimLine != null) _aimLine.enabled = true;
        }

        if (isAiming)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - transform.position);
            direction.Normalize();
            shootDirection = direction;

            float offset = 0.6f;
            float lineLength = 5f;
            Vector2 startPos = (Vector2)transform.position + direction * offset;
            Vector2 endPos = startPos + direction * lineLength;

            if (_aimLine != null)
            {
                _aimLine.SetPosition(0, startPos);
                _aimLine.SetPosition(1, endPos);
            }
        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            isAiming = false;
            if (_aimLine != null) _aimLine.enabled = false;
            ShootBall();
        }
    }

    void FixedUpdate()
    {
        // after shot, keep a constant speed
        if (!isReady && rb != null && rb.velocity != Vector2.zero)
        {
            rb.velocity = _constantSpeed * (rb.velocity.normalized);
        }
    }

    void ShootBall()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * _ballSpeed;
        isReady = false;
    }

    public void SetReady(bool ready)
    {
        isReady = ready;
        if (ready && rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}

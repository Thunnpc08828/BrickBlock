using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _constantSpeed = 10f; // keep speed constant
    [SerializeField] private float _ballSpeed = 10f;     // shoot speed
    [SerializeField] private LineRenderer _aimLine;      // assign in prefab/inspector

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Vector2 _shootDirection;
    [SerializeField] private bool _isAiming = false;
    [SerializeField] private bool _isReady = true;


    void Start()
    {

    }

    void Update()
    {
        if (!_isReady) return;

        if (Input.GetMouseButtonDown(0))
        {
            _isAiming = true;
            if (_aimLine != null) _aimLine.enabled = true;
        }

        if (_isAiming)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - transform.position);
            direction.Normalize();
            _shootDirection = direction;

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

        if (Input.GetMouseButtonUp(0) && _isAiming)
        {
            _isAiming = false;
            if (_aimLine != null) _aimLine.enabled = false;
            ShootBall();
        }
    }

    void FixedUpdate()
    {
        // after shot, keep a constant speed
        if (!_isReady && _rb != null && _rb.velocity != Vector2.zero)
        {
            _rb.velocity = _constantSpeed * (_rb.velocity.normalized);
        }
    }

    void ShootBall()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _shootDirection * _ballSpeed;
        _isReady = false;
    }

    public void SetReady(bool ready)
    {
        _isReady = ready;
        if (ready && _rb != null)
        {
            _rb.velocity = Vector2.zero;
        }
    }
}

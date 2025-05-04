using UnityEngine;

public class EnemyAIPatrol : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 3f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _movement;
    private Transform _targetPoint;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        if (pointA == null || pointB == null)
            Debug.LogError($"[{name}] Patrol points not set!");

        // start by moving toward point B
        _targetPoint = pointB;
    }

    private void FixedUpdate()
    {
        // 1) compute direction toward the current target point
        Vector2 dir = ((Vector2)_targetPoint.position - _rb.position).normalized;

        // 2) move
        _movement = dir;
        _rb.velocity = dir * speed;

        // 3) if close enough, swap target
        if (Vector2.Distance(_rb.position, _targetPoint.position) < 0.1f)
        //if targepoint == pointA then targetpoint = b else it equals A
            _targetPoint = (_targetPoint == pointA) ? pointB : pointA;
    }

    private void Update()
    {
        // drive the same animator floats you used on the player
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical,_movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
        }
    }
}

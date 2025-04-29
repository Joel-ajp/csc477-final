using UnityEngine;

public class EnemyAIFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 3f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _movement;

    private Knockback knockback;

    private const string _horizontal="Horizontal";
    private const string _vertical="Vertical";
    private const string _lastHorizontal="LastHorizontal";
    private const string _lastVertical="LastVertical";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();

        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
                player = go.transform;
            else
                Debug.LogError($"[{name}] couldn’t find any GameObject tagged ‘Player’!");
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if(knockback.gettingKnockedBack) {return;}

        // 1) Calculate normalized direction toward player
        Vector2 dir = (player.position - transform.position).normalized;

        // 2) Cache for animation and drive physics
        _movement = dir;
        _rb.velocity = dir * speed;
    }

    private void Update()
    {
        // 3) Feed the animator exactly like the player does
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
        }
    }
}

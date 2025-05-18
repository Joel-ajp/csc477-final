using System.Collections;
using UnityEngine;


public class EnemyAITeloprt : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float invisibilityDuration = 1.5f;
    [SerializeField] private float postAppearDelay = 0.8f;
    [SerializeField] private float followDuration = 0.5f;
    [SerializeField] private float teleportRadius = 5f;
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float stopAnimation = 4f;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sr;
    private CapsuleCollider2D _col;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<CapsuleCollider2D>();

        if (player == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
            else Debug.LogError($"[{name}] No GameObject tagged 'Player' found!");
        }
    }

    private void OnEnable()
    {
        StartCoroutine(TeleportFollowLoop());
    }

    private IEnumerator TeleportFollowLoop()
    {
        while (true)
        {
            // 2) Go invisible & pause animator
            _sr.enabled = false;
            _anim.enabled = false;
            _col.enabled = false;

            float timer = 0f;
            bool doneTeleport = false;
            while (timer < invisibilityDuration)
            {
                if (!doneTeleport)
                {
                    // perform teleport , while still hidden
                    Vector2 dest = (Vector2)player.position + Random.insideUnitCircle * teleportRadius;
                    _rb.position = dest;
                    _rb.velocity = Vector2.zero;
                    doneTeleport = true;
                }

                timer += Time.deltaTime;
                yield return null;  // wait one frame, stay hidden
            }

            // 4) Now that invisibility window is over, re-show & animate
            _sr.enabled = true;
            _anim.enabled = true;
            _col.enabled = true;
            yield return new WaitForSeconds(postAppearDelay);

            // 8) Follow the player
            float elapsed = 0f;
            while (elapsed < followDuration)
            {
                Vector2 dir = ((Vector2)player.position - _rb.position).normalized;

                // animate
                _anim.SetFloat(_horizontal, dir.x);
                _anim.SetFloat(_vertical, dir.y);
                _anim.SetFloat(_lastHorizontal, dir.x);
                _anim.SetFloat(_lastVertical, dir.y);

                // move
                _rb.velocity = dir * followSpeed;

                elapsed += Time.deltaTime;
                yield return null;
            }
            // 4) Stop animation and wait before disappearing
            _rb.velocity = Vector2.zero;
            _anim.SetFloat(_horizontal, 0f);
            _anim.SetFloat(_vertical, 0f);

            float stopTimer = 0f;
            while (stopTimer < stopAnimation)
            {
                stopTimer += Time.deltaTime;
                yield return null;
            }

            // 5) Go invisible for the next teleport cycle
            _sr.enabled = false;
            _anim.enabled = false;
            _col.enabled = false;
        }
    }

}




using System.Collections;
using UnityEngine;

public class BossEnemyAI : MonoBehaviour
{
    private enum State { Follow, Teleporting }

    [Header("Shard Drop")]
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private Transform shardSpawnPoint;
    private bool shardDropped = false;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemyHealth health;

    [Header("Phase-Switch")]
    [SerializeField] private int phaseTwoHealthThreshold = 1;
    private bool hasEnteredPhaseTwo = false;

    [Header("Follow & Attack")]
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float attackDuration = 1f;  // total length of the swing anim

    private bool _isAttacking = false;

    [Header("Teleport Phase")]
    [SerializeField] private float invisibilityDuration = 1.5f;
    [SerializeField] private float postAppearDelay = 0;
    [SerializeField] private float teleportFollowDuration = 5f;
    [SerializeField] private float teleportRadius = 5f;
    [SerializeField] private float stopAnimationTime = 4f;

    // Animator parameters
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorz = "LastHorizontal";
    private const string _lastVert = "LastVertical";
    private const string _attackBool = "Attack";

    private State currentState = State.Follow;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr  = GetComponent<SpriteRenderer>();

        if (player == null) player = GameObject.FindWithTag("Player")?.transform;
        if (health == null) health = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        // 1) Death & shard drop
        if (!shardDropped && health.CurrentHealth <= 0)
        {
            DropShard();
            shardDropped = true;
            Destroy(gameObject);
            return;
        }

        // 2) Switch to teleport phase
        if (!hasEnteredPhaseTwo && health.CurrentHealth <= phaseTwoHealthThreshold)
        {
            hasEnteredPhaseTwo = true;
            currentState = State.Teleporting;
            StopAllCoroutines();
            StartCoroutine(TeleportPhase());
            return;
        }

        // 3) Follow or attack
        if (currentState == State.Follow)
        {
            // if already swinging, ignore follow/attack checks
            if (_isAttacking)
                return;

            FollowPlayer();

            float dist = Vector2.Distance(rb.position, player.position);
            if (dist < detectionRadius)
            {
                // start the swing coroutine
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            // ensure attack flag is reset if we teleport away mid‐swing
            anim.SetBool(_attackBool, false);
            _isAttacking = false;
        }
    }

    private void FollowPlayer()
    {
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        rb.velocity = dir * followSpeed;
        SetAnimation(dir);
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;

        // stop movement
        rb.velocity = Vector2.zero;
        // enter attack anim
        anim.SetBool(_attackBool, true);

        // wait halfway if you need to time any “hit” effect here:
        yield return new WaitForSeconds(attackDuration * 0.5f);

        // deal damage / spawn hit effect / etc.

        // finish the rest of the animation
        yield return new WaitForSeconds(attackDuration * 0.5f);

        // exit attack anim
        anim.SetBool(_attackBool, false);
        _isAttacking = false;
    }

    private IEnumerator TeleportPhase()
    {
        while (true)
        {
            // invisibility (commented out)
            // sr.enabled = false;
            // anim.enabled = false;

            float timer = 0f;
            bool teleported = false;
            while (timer < invisibilityDuration)
            {
                if (!teleported)
                {
                    Vector2 dest = (Vector2)player.position + Random.insideUnitCircle * teleportRadius;
                    rb.position = dest;
                    rb.velocity = Vector2.zero;
                    teleported = true;
                }
                timer += Time.deltaTime;
                yield return null;
            }

            sr.enabled  = true;
            anim.enabled = true;

            float elapsed = 0f;
            while (elapsed < teleportFollowDuration)
            {
                Vector2 dir = ((Vector2)player.position - rb.position).normalized;
                rb.velocity = dir * followSpeed;
                SetAnimation(dir);
                   float dist = Vector2.Distance(rb.position, player.position);
            if (dist < detectionRadius)
            {
                // start the swing coroutine
                StartCoroutine(AttackRoutine());
            }
                elapsed += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;
        }
    }

    private void SetAnimation(Vector2 dir)
    {
        anim.SetFloat(_horizontal, dir.x);
        anim.SetFloat(_vertical, dir.y);
        if (dir != Vector2.zero)
        {
            anim.SetFloat(_lastHorz, dir.x);
            anim.SetFloat(_lastVert, dir.y);
        }
    }

    private void DropShard()
    {
        if (shardPrefab == null)
        {
            Debug.LogWarning($"[{name}] No shard prefab assigned!");
            return;
        }
        Vector3 spawnPos = shardSpawnPoint != null ? shardSpawnPoint.position : transform.position;
        Instantiate(shardPrefab, spawnPos, Quaternion.identity);
    }
}

using System.Collections;
using UnityEngine;

public class BossEnemyAI : MonoBehaviour
{
    private enum State { Follow, Charging, Teleporting }
    private State currentState = State.Follow;
    private bool hasEnteredPhaseTwo = false;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemyHealth health;

    [Header("Phase-Switch")]
    [SerializeField] private int phaseTwoHealthThreshold = 1;
    
    [Header("Follow")]
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float detectionRadius = 5f;

    [Header("Charge Cycle")]
    [SerializeField] private float chargePrepTime = 0.5f;
    [SerializeField] private float chargeSpeed = 8f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float retreatSpeed = 3f;
    [SerializeField] private float retreatDistance = 4f;
    [SerializeField] private float recoverTime = 1f;

    // Commented out animator parameters for now
    // private const string _horizontal = "Horizontal";
    // private const string _vertical = "Vertical";
    // private const string _lastHorizontal = "LastHorizontal";
    // private const string _lastVertical = "LastVertical";

    [Header("Teleport Phase")]
    [SerializeField] private float invisibilityDuration = 1.5f;
    [SerializeField] private float postAppearDelay = 0.8f;
    [SerializeField] private float teleportFollowDuration = 0.5f;
    [SerializeField] private float teleportRadius = 5f;
    [SerializeField] private float stopAnimationTime = 4f;

    private Rigidbody2D rb;
    // Commented out animator and sprite renderer for now
    // private Animator anim;
    // private SpriteRenderer sr;
    private Vector2 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>();
        // sr = GetComponent<SpriteRenderer>();

        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        if (health == null)
            health = GetComponent<EnemyHealth>();

        // if (anim == null)
        //     Debug.LogWarning($"[{name}] No Animator found on boss.");
    }

    private void Update()
    {
        // Phase-2 trigger
        if (!hasEnteredPhaseTwo && health.CurrentHealth <= phaseTwoHealthThreshold)
        {
            hasEnteredPhaseTwo = true;
            currentState = State.Teleporting;
            StopAllCoroutines();
            StartCoroutine(TeleportPhase());
            return;
        }

        if (currentState == State.Follow)
        {
            FollowPlayer();

            if (Vector2.Distance(rb.position, player.position) < detectionRadius)
                StartCoroutine(ChargeRoutine());
        }
    }

    private void FollowPlayer()
    {
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        movement = dir;
        rb.velocity = dir * followSpeed;
        // Animation disabled for now
        // SetAnimation(dir);
    }

    private IEnumerator ChargeRoutine()
    {
        currentState = State.Charging;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(chargePrepTime);

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        float t = 0f;
        while (t < chargeDuration)
        {
            // SetAnimation(dir);
            rb.velocity = dir * chargeSpeed;
            t += Time.deltaTime;
            yield return null;
        }

        Vector2 retreat = -dir;
        while (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            // SetAnimation(retreat);
            rb.velocity = retreat * retreatSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(recoverTime);

        currentState = State.Follow;
    }

    private IEnumerator TeleportPhase()
    {
        while (true)
        {
            // go invisible & pause animation (disabled)
            // sr.enabled = false;
            // if (anim != null)
            //     anim.enabled = false;

            float timer = 0f;
            bool doneTeleport = false;
            while (timer < invisibilityDuration)
            {
                if (!doneTeleport)
                {
                    Vector2 dest = (Vector2)player.position + Random.insideUnitCircle * teleportRadius;
                    rb.position = dest;
                    rb.velocity = Vector2.zero;
                    doneTeleport = true;
                }
                timer += Time.deltaTime;
                yield return null;
            }

            // re-show animation (disabled)
            // sr.enabled = true;
            // if (anim != null)
            //     anim.enabled = true;
            yield return new WaitForSeconds(postAppearDelay);

            // follow burst
            float elapsed = 0f;
            while (elapsed < teleportFollowDuration)
            {
                Vector2 dir = ((Vector2)player.position - rb.position).normalized;
                // Animation disabled
                rb.velocity = dir * followSpeed;
                elapsed += Time.deltaTime;
                yield return null;
            }

            // stop and idle
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(stopAnimationTime);

            // go invisible for next cycle (disabled)
            // sr.enabled = false;
            // if (anim != null)
            //     anim.enabled = false;
        }
    }

    // Animation method commented out
    // private void SetAnimation(Vector2 dir)
    // {
    //     anim.SetFloat(_horizontal, dir.x);
    //     anim.SetFloat(_vertical, dir.y);
    //     if (dir != Vector2.zero)
    //     {
    //         anim.SetFloat(_lastHorizontal, dir.x);
    //         anim.SetFloat(_lastVertical, dir.y);
    //     }
    // }
}
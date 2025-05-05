using System.Collections;
using UnityEngine;

public class EnemyAICharge : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chargeSpeed = 1f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float retreatSpeed = 3f;
    [SerializeField] private float retreatDistance = 4f;
    [SerializeField] private float recoverTime = 1f;
    [SerializeField] private float chargePrepTime= 5f; 
    [SerializeField] private float detectionRadius = 5f;
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";
    private Animator _anim;
    private SpriteRenderer _sr;

    private Rigidbody2D rb;
    private bool isBusy = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
         _anim = GetComponent<Animator>();  
        if (player == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
            else Debug.LogError($"[{name}] No GameObject tagged 'Player' found!");
        }
    }

    // This fires the moment something enters the trigge
    private void Update()
{
    if (isBusy) return;
    
    float dist = Vector2.Distance(transform.position, player.position);
    if (dist < detectionRadius)
        StartCoroutine(ChargeAndRetreatRoutine());
}

    private IEnumerator ChargeAndRetreatRoutine()
    {
        isBusy = true;
         rb.velocity = Vector2.zero;
         float prepTimer = 0f;
           while (prepTimer < chargePrepTime){
            prepTimer += Time.deltaTime;
            yield return null;   // wait one frame
            }

        // Charge Phase
         Vector2 dir = (player.position - transform.position).normalized;
        float  timer = 0f;
        while (timer < chargeDuration)
        {
            _anim.SetFloat(_horizontal,dir.x);
            _anim.SetFloat(_vertical,dir.y);
            _anim.SetFloat(_lastHorizontal,dir.x);
            _anim.SetFloat(_lastVertical,dir.y);

            rb.velocity = dir * chargeSpeed;
            timer  += Time.deltaTime;
            yield return null;
        }

        // Retreat Phase
        Vector2 retreatDir = -dir;
        while (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            _anim.SetFloat(_horizontal,retreatDir.x);
            _anim.SetFloat(_vertical,retreatDir.y);
            _anim.SetFloat(_lastHorizontal,retreatDir.x);
            _anim.SetFloat(_lastVertical,retreatDir.y);

            rb.velocity = retreatDir * retreatSpeed;
            yield return null;
        }

        //Stop movement
        rb.velocity = Vector2.zero;
        _anim.SetFloat(_horizontal, 0f);
        _anim.SetFloat(_vertical, 0f);

        // Recovery Phase
        yield return new WaitForSeconds(recoverTime);
        isBusy = false;
    }
}

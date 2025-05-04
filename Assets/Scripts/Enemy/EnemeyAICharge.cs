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

    private Rigidbody2D rb;
    private bool isBusy = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
         float prepTimer = 0f;
           while (prepTimer < chargePrepTime)
    {
        prepTimer += Time.deltaTime;
        yield return null;   // wait one frame
    }

        // Charge Phase
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        float  timer = 0f;
        while (timer < chargeDuration)
        {
            rb.velocity = dirToPlayer * chargeSpeed;
            timer  += Time.deltaTime;
            yield return null;
        }

        // Retreat Phase
        Vector2 retreatDir = -dirToPlayer;
        while (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            rb.velocity = retreatDir * retreatSpeed;
            yield return null;
        }

        //Stop movement
        rb.velocity = Vector2.zero;

        // Recovery Phase
        yield return new WaitForSeconds(recoverTime);
        isBusy = false;
    }
}

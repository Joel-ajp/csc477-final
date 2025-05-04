using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor   = spriteRenderer.color;
        enemyHealth    = GetComponent<EnemyHealth>();

        if (spriteRenderer == null)
            Debug.LogError($"[{name}] Flash needs a SpriteRenderer!");
        if (enemyHealth == null)
            Debug.LogError($"[{name}] Flash needs an EnemyHealth component!");
    }

    public IEnumerator FlashRoutine()
    {
        // 1) flash white
        spriteRenderer.color = Color.red;

        // 2) wait
        yield return new WaitForSeconds(flashDuration);

        // 3) restore and then check for death
        spriteRenderer.color = defaultColor;
        enemyHealth.DetectDeath();
    }
}

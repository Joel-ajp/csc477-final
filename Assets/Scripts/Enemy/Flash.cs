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
        defaultColor =  spriteRenderer.material.color ;
        enemyHealth = GetComponent<EnemyHealth>();

        if (spriteRenderer == null)
            Debug.LogError($"[{name}] Flash needs a SpriteRenderer!");
        if (enemyHealth == null)
            Debug.LogError($"[{name}] Flash needs an EnemyHealth component!");
    }

    public IEnumerator FlashRoutine()
    {
        // 1) flash white
        spriteRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        // 2) wait
        yield return new WaitForSeconds(flashDuration);

        // 3) restore and then check for death
        spriteRenderer.material.color = defaultColor;
        enemyHealth.DetectDeath();
    }
}

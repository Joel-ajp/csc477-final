using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private PlayerStats stats;

    private void Awake()
    {
        // Find the Player by tag and cache its stats
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            stats = player.GetComponent<PlayerStats>();
            if (stats == null)
                Debug.LogError("[DamageEnemy] No PlayerStats on the Player object!", player);
        }
        else
        {
            Debug.LogError("[DamageEnemy] No GameObject tagged 'Player' found in scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null && stats != null)
        {
            int totalDamage = damageAmount + stats.attack_damage;
            enemy.TakeDamage(totalDamage);
            // where we could some indication of destruction of arrow
            Destroy(gameObject);
        }
    }
}

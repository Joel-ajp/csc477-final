
using UnityEngine;
public class SpawnEnemies : MonoBehaviour
{
    [Tooltip("Enemy prefab to spawn")]
    public GameObject enemyPrefab;
    public int spawnCount = 3;
    public Transform[] spawnPoints;

    private bool hasTriggered = false;
    private BoxCollider2D _col;

    void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
        _col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        // assumes your Player has the “Player” tag
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // spawn at each point (clamped by spawnCount)
            for (int i = 0; i < Mathf.Min(spawnCount, spawnPoints.Length); i++)
            {
                     GameObject spawned = Instantiate(enemyPrefab,spawnPoints[i].position,Quaternion.identity);
                     spawned.SetActive(true);
            }
        }
        else
        {
            // random positions inside the trigger bounds
            Bounds b = _col.bounds;
            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 pos = new Vector2(
                    Random.Range(b.min.x, b.max.x),
                    Random.Range(b.min.y, b.max.y)
                );
                GameObject spawned = Instantiate(enemyPrefab, pos, Quaternion.identity);
                spawned.SetActive(true);
            }
        }
    }
}
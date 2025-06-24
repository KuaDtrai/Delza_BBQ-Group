using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab here
    public float spawnRate = 3f; // Time in seconds between spawns
    public float spawnRadius = 0f; // Radius around the spawner to spawn enemies
    private float nextSpawnTime;

    void Update()
    {
        
        if (Time.time >= nextSpawnTime)
        {            
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnEnemy()
    {
        // Generate a random offset within the spawn radius
        Vector2 spawnOffset = Random.insideUnitCircle * spawnRadius;
        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;

        // Instantiate the enemy at the calculated position and rotation
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    public List<Enemy> SpawnableEnemies;
    public float minSpawnRadius;
    public float spawnSpread;
    public int enemySpawnCount;
    private List<Enemy> SpawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns more enemies
    void SpawnEnemies()
    {
        // Spawn a single enemy
        for (int i = 0; i < enemySpawnCount; i++)
        {
            SpawnSingleEnemy();
        }
    }
    
    // Spawns one enemy at a time.  Used by SpawnEnemies()
    void SpawnSingleEnemy() {
        // Prioritize spawning an unactive enemy
        // If no such enemy exists, instantiate a new one
    }

    // Pick a random location, used for picking where to spawn enemies
    Vector2 RandPos() {
        // Pick random angle
        float angle = Random.Range(0.0f, 2*Mathf.PI);

        // Pick a random distance above the minimum spawn distance.
        // This is weighted to favor spawning closer to the minimum, depending on the current value of "spread"
        float distance = minSpawnRadius * (1 + Mathf.Exp(-Random.Range(0.0f, 5.0f)/spawnSpread));

        // Convert to vector
        return new Vector2(distance*Mathf.Cos(angle), distance*Mathf.Sin(angle));
    }
}

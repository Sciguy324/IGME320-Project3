using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{   
    public List<Enemy> SpawnableEnemies;
    public float minSpawnRadius;
    public float spawnSpread;
    public int enemySpawnCount;
    public GenericEntity SurroundEntity;
    private List<Enemy> SpawnedEnemies = new List<Enemy>();
    public int currentEnemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Test spawn
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        SpawnEnemiesCount(enemySpawnCount);
    }

    //Option for spawning spesific amount
    public void SpawnEnemiesCount(int enemiesToSpawn)
    {

        int toSpawn = enemiesToSpawn;
        Vector2 spawnCenter = SurroundEntity.transform.position;

        // Recycle existing gameobjects first
        if (SpawnableEnemies.Count > 0)
        {
            foreach (Enemy entity in SpawnedEnemies)
            {
                if (!entity.gameObject.activeSelf)
                {
                    if (currentEnemyCount > 100)
                    {
                        return;
                    }
                    toSpawn--;
                    Vector2 respawnPos = RandPos(spawnCenter);

                    entity.Respawn(respawnPos);
                    currentEnemyCount++;
                }

                if (toSpawn == 0)
                {
                    // We've spawned enough
                    return;
                }
            }
        }
        // Instantiate new entities if we need more

        // Spawn a single enemy
        for (int i = 0; i < toSpawn; i++)
        {
            if (currentEnemyCount > 100)
            {
                return;
            }
            // Randomly pick enemy type and position
            int randIndex = Random.Range(0, SpawnableEnemies.Count);
            Debug.Log(SpawnableEnemies[randIndex].name);
            Enemy newEnemy = Instantiate(SpawnableEnemies[randIndex], RandPos(spawnCenter), Quaternion.identity);
            newEnemy.targetEntity = SurroundEntity;

            // Add to list
            SpawnedEnemies.Add(newEnemy);
            currentEnemyCount++;

        }
    }

    // Pick a random location, used for picking where to spawn enemies
    Vector2 RandPos(Vector2 center) {
        // Pick random angle
        float angle = Random.Range(0.0f, 2*Mathf.PI);

        // Pick a random distance above the minimum spawn distance.
        // This is weighted to favor spawning closer to the minimum, depending on the current value of "spread"
        float distance = minSpawnRadius * (1 + Mathf.Exp(-Random.Range(0.0f, 5.0f)/spawnSpread));

        // Convert to vector
        return (center + new Vector2(distance*Mathf.Cos(angle), distance*Mathf.Sin(angle)));
    }
}

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
    private List<Enemy> SpawnedEnemiesLevel1 = new List<Enemy>();
    private List<Enemy> SpawnedEnemiesLevel2 = new List<Enemy>();
    private List<Enemy> SpawnedEnemiesLevel3 = new List<Enemy>();
    private List<List<Enemy>> enemiesLists;
    public int enemyLevelToSpawn = 0;

    public int currentEnemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //setting up spawn list arr
        enemiesLists = new List<List<Enemy>>() { SpawnedEnemiesLevel1, SpawnedEnemiesLevel2, SpawnedEnemiesLevel3 };
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
        List<Enemy> currentSpawnList = enemiesLists[enemyLevelToSpawn];

        int toSpawn = enemiesToSpawn;
        Vector2 spawnCenter = SurroundEntity.transform.position;

        // Recycle existing gameobjects first
        if (SpawnableEnemies.Count > 0)
        {
            foreach (Enemy entity in currentSpawnList)
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
          
            Enemy newEnemy = Instantiate(SpawnableEnemies[enemyLevelToSpawn], RandPos(spawnCenter), Quaternion.identity);
            newEnemy.targetEntity = SurroundEntity;

            // Add to list
            currentSpawnList.Add(newEnemy);
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

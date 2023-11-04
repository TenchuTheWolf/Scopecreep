using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevHellMode : MonoBehaviour
{
    public static DevHellMode instance;

    public float enemySpawnInterval = 3f;
    public float defaultEnemySpawnInterval = 3f;
    public float intervalDecrimentAmount = 0.1f;
    public float minimumEnemySpawnInterval = 0.1f;
    public int enemySpawnCount = 3;
    public static bool devHellModeActive = false;

    public List<NPC> enemySpawnOptions = new List<NPC>();

    public void Awake()
    {
        instance = this;
    }
    public void DecreaseEnemySpawnInterval()
    {
        enemySpawnInterval -= intervalDecrimentAmount;
        if (enemySpawnInterval < minimumEnemySpawnInterval)
        {
            enemySpawnInterval = minimumEnemySpawnInterval;
        }
    }


    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnCount; i++)
        {
            int randomIndex = Random.Range(0, enemySpawnOptions.Count);

            GameManager.gameManagerInstance.SpawnASingleEnemy(enemySpawnOptions[randomIndex]);

        }

        enemySpawnCount++;
        DecreaseEnemySpawnInterval();
        StartCoroutine(SpawnHellTimer());
    }

    public IEnumerator SpawnHellTimer()
    {
        if(Player.playerInstance == null)
        {
            yield break;
        }

        WaitForSeconds waiter = new WaitForSeconds(enemySpawnInterval);
        yield return waiter;

        SpawnEnemies();
    }

    /// <summary>
    /// Resetting the spawn interval ensures Dev Hell doesn't immediately spawn 500 enemies if you reach Devhell on a second playthrough.
    /// </summary>
    public void CleanUpDevHell()
    {
        StopAllCoroutines();
        enemySpawnInterval = defaultEnemySpawnInterval;
        enemySpawnCount = 3;
    }


}

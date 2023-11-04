using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;


    public List<Transform> spawnLocations = new List<Transform>();
    public List<GameObject> allPossibleLoot = new List<GameObject>();

    public float lootScoreSpawnThreshold = 10f;
    public float currentLootScore = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public static void AddToThreshold(float points)
    {
        instance.currentLootScore += points;
        if(instance.currentLootScore >= instance.lootScoreSpawnThreshold)
        {
            instance.SpawnLoot();
            instance.currentLootScore = instance.currentLootScore - instance.lootScoreSpawnThreshold;
        }
    }

    public void SpawnLoot()
    {
        int spawnIndex = Random.Range(0, spawnLocations.Count);
        int lootIndex = Random.Range(0, allPossibleLoot.Count);
        Instantiate(allPossibleLoot[lootIndex], spawnLocations[spawnIndex].position, Quaternion.identity);
    }

    public static void ResetLootScore()
    {
        instance.currentLootScore = 0f;
    }

}

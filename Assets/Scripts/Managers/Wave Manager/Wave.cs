using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<WaveEntry> waveEntries = new List<WaveEntry>();


    public void SpawnWave()
    {
        for (int i = 0; i < waveEntries.Count; i++)
        {
            waveEntries[i].Spawn();
        }
    }
}

[System.Serializable]
public class WaveEntry
{
    public int count;
    public NPC enemy;

    public void Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            List<Transform> locations = GameManager.gameManagerInstance.enemySpawnLocations;

            int randomIndex = Random.Range(0, locations.Count);

            GameObject.Instantiate(enemy, locations[randomIndex].position, Quaternion.identity);
        }
    }

}

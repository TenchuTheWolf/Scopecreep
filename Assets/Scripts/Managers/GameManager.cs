using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

using EnemyType = NPC.EnemyType;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;
    public Canvas toolTipCanvas;
    public Player playerPrefab;
    //public int startingAsteroidCount = 3;
    //public Transform asteroidSpawnLocation;
    public WaveManager waveManager;
    private static List<NPC> activeEnemies = new List<NPC>();
    public Dictionary<EnemyType, List<DogTagData>> killList = new Dictionary<EnemyType, List<DogTagData>>();
    public List<Transform> enemySpawnLocations = new List<Transform>();
    public List<Pickup> pickupList = new List<Pickup>();


    public static bool AreEnemiesActive { get { return activeEnemies.Count > 0; } }

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
        }
    }

    public void StartGame()
    {
        DevHellMode.devHellModeActive = false;
 
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].CleanupHealthBar();
        }

        waveManager.ResetWaveCount();
        CooldownUIManager.instance.ClearCooldownEntries();
        UpgradeManager.instance.purchasedUpgrades.Clear();
        RemoveAllEnemies();
        ClearPickups();
        GameTaskManager.instance.ResetEvent();
        LootManager.ResetLootScore();

        Instantiate(playerPrefab, Vector2.zero, Quaternion.identity);
        PanelManager.OpenPanel<HUD>();
        HUD targetHUD = PanelManager.GetPanel<HUD>();
        targetHUD.UpdatePlayerUIHealthBar(1);
        targetHUD.UpdateCurrencyDisplay(0);

        UpgradeManager.instance.ResetAllUpgrades();
        DevHellMode.instance.CleanUpDevHell();

        StartCoroutine(SpawnNextWaveOnDelay());

        //for (int i = 0; i < waveManager.currentWave; i++)
        //{
        //    Instantiate(asteroidPrefab, asteroidSpawnLocation.position, Quaternion.identity);
        //}
    }

    private IEnumerator SpawnNextWaveOnDelay()
    {
        if(Player.playerInstance != null)
        {
            Player.playerInstance.EnableEnhancedVacuum();
        }

        WaitForSeconds waiter = new WaitForSeconds(2f);

        if (GameTaskManager.instance.CheckTaskEventStart(waveManager.nextWave) == true)
        {
            waveManager.nextWave++;

            if (Player.playerInstance != null)
            {
                Player.playerInstance.ResetDefaultVacuum();
            }
            yield break;
        }

        PanelManager.OpenPanel<BroadcastPanel>().ShowMessage("Sprint " + (waveManager.nextWave + 1) + " is starting soon! Get ready!");

        yield return waiter;

        if (Player.playerInstance != null)
        {
            Player.playerInstance.ResetDefaultVacuum();
        }
        waveManager.SpawnWave(waveManager.nextWave);
    }

    public void SpawnEnemies(int waveOverride = 0)
    {
        waveManager.SpawnWave(waveOverride);
    }

    public void RemoveAllEnemies()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Destroy(activeEnemies[i].gameObject);
        }
        activeEnemies.Clear();
        killList.Clear();
    }

    public static void EnlistEnemy(NPC target)
    {
        if (activeEnemies.Contains(target) == false)
        {
            //Debug.Log("Tried to add " + target.gameObject.name);

            activeEnemies.Add(target);
        }
        else
        {
            Debug.LogError("Tried to add an enemy to a list a second time for some reason.");
        }
    }

    public static void UnlistEnemy(NPC target)
    {
        if (activeEnemies.Contains(target) == true)
        {
            activeEnemies.Remove(target);
        }
        else
        {
            Debug.LogError("Tried to remove an enemy that was not in the list.");
        }

        if (gameManagerInstance.killList.ContainsKey(target.enemyType) == false)
        {
            //List<NPC> enemyCountByType = new List<NPC>();
            //enemyCountByType.Add(target);


            List<DogTagData> dogTagList = new List<DogTagData>();
            DogTagData targetDogTag = new DogTagData(target.enemyType, target.npcSpriteRenderer.sprite);
            dogTagList.Add(targetDogTag);
            gameManagerInstance.killList.Add(target.enemyType, dogTagList);
        }
        else
        {
            gameManagerInstance.killList[target.enemyType].Add(new DogTagData(target.enemyType, target.npcSpriteRenderer.sprite));
        }

        if (activeEnemies.Count == 0)
        {
            //Debug.LogWarning("Starting next wave");
            //This is to prevent the last remaining enemy being unlisted trying to summon the UpgradeMenu/Starting Next Wave during Dev Hell.
            if (DevHellMode.devHellModeActive == false)
            {
                gameManagerInstance.StartCoroutine(gameManagerInstance.SpawnNextWaveOnDelay());
            }
        }
    }

    public NPC SpawnASingleEnemy(NPC enemyToSpawn)
    {
        int randomSpawnIndex = Random.Range(0, enemySpawnLocations.Count);
        NPC selectedEnemy = GameObject.Instantiate(enemyToSpawn, enemySpawnLocations[randomSpawnIndex].position, Quaternion.identity);
        return selectedEnemy;
    }

    private void ClearPickups()
    {
        for (int i = 0; i < pickupList.Count; i++)
        {
            if (pickupList[i] == null)
            {
                continue;
            }
            Destroy(pickupList[i].gameObject);
        }
        pickupList.Clear();
        
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameTaskManager : MonoBehaviour
{
    public static GameTaskManager instance;

    public List<Transform> taskSpawnLocations = new List<Transform>();
    public int TasksToComplete { get; private set; }

    public CrunchTimeEvent crunchtimeEventPrefab;
    public DeadlineEvent deadlineEventPrefab;

    public GameEvent currentGameEvent;

    public List<NPC> eventEnemyPrefabs = new List<NPC>();
    private List<NPC> activeEventEnemies = new List<NPC>();

    public static bool EventActive { get { return instance.currentGameEvent != null; } }

    private List<TaskPickup> activeTasks = new List<TaskPickup>();

    private void Awake()
    {
        instance = this;
    }

    public bool CheckTaskEventStart(int waveCount)
    {
        if (waveCount % 3 == 0 && waveCount != 0)
        {
            StartTaskEvent(waveCount);
            return true;
        }
        return false;
    }

    private void StartTaskEvent(int waveCount)
    {
        int randomEvent = Random.Range(0, 2);
        if (randomEvent == 0)
        {
            DeadlineEvent deadlineEvent = Instantiate(deadlineEventPrefab);
            currentGameEvent = deadlineEvent;
            deadlineEvent.LaunchDeadlineEvent(waveCount);
        }
        else
        {
            CrunchTimeEvent crunchtimeEvent = Instantiate(crunchtimeEventPrefab);
            currentGameEvent = crunchtimeEvent;
            crunchtimeEvent.LaunchCrunchTimeEvent(waveCount);
        }
        for (int i = 0; i < eventEnemyPrefabs.Count; i++)
        {
            NPC spawnedEnemy = GameManager.gameManagerInstance.SpawnASingleEnemy(eventEnemyPrefabs[i]);
            activeEventEnemies.Add(spawnedEnemy);
            //Debug.Log("Spawned enemy: " + spawnedEnemy.gameObject.name + " is being added.");
        }

    }

    public void EnlistTask(TaskPickup taskToEnlist)
    {
        activeTasks.Add(taskToEnlist);
    }

    public void DelistTask(TaskPickup taskToDelist)
    {
        activeTasks.Remove(taskToDelist);
    }

    public void ClearTaskList()
    {
        for (int i = 0; i < activeTasks.Count; i++)
        {
            if (activeTasks[i] == null)
            {
                continue;
            }
            Destroy(activeTasks[i].gameObject);
        }
        activeTasks.Clear();
    }


    public void EndTaskEvent(GameEvent eventToEnd)
    {
        for (int i = 0; i < activeEventEnemies.Count; i++)
        {

            if (activeEventEnemies[i] != null)
            {
                StartCoroutine(activeEventEnemies[i].DieOnDelay());
            }
        }



        activeEventEnemies.Clear();

        Destroy(eventToEnd.gameObject);
        currentGameEvent = null;
        ClearTaskList();
        if (GameManager.AreEnemiesActive == false)
        {
            PanelManager.OpenPanel<UpgradePanel>();
        }

        StartCoroutine(Player.playerInstance.CycleVacuum());

    }

    public void StartMicroManagerRecursion()
    {
        StartCoroutine(SpawnUntrackedMicromanager());
    }

    private IEnumerator SpawnUntrackedMicromanager()
    {
        yield return new WaitForSeconds(3f);
        if (currentGameEvent != null)
        {
            NPC targetNPC = GameManager.gameManagerInstance.SpawnASingleEnemy(eventEnemyPrefabs[0]);
            activeEventEnemies.Add(targetNPC);
        }
    }


    public void TaskCollected(TaskPickup taskPickup)
    {
        if (currentGameEvent == null)
        {
            return;
        }
        currentGameEvent.TaskCompleted();
        DelistTask(taskPickup);
    }

    public void TaskFizzled(TaskPickup taskFizzled)
    {
        if (currentGameEvent == null)
        {
            return;
        }
        currentGameEvent.TaskMissed(taskFizzled);
        DelistTask(taskFizzled);
    }


    public void SpawnTask(TaskPickup targetTask, int amountOfTasks, bool multipleTasks)
    {
        List<Transform> trackedSpawnPoints = new List<Transform>(taskSpawnLocations);

        for (int i = 0; i < amountOfTasks; i++)
        {
            int spawnIndex = Random.Range(0, trackedSpawnPoints.Count);
            Transform spawnLocation = trackedSpawnPoints[spawnIndex];
            Instantiate(targetTask, spawnLocation.position, Quaternion.identity);
            trackedSpawnPoints.Remove(spawnLocation);
        }
    }

    public void ResetEvent()
    {
        if (currentGameEvent != null)
        {
            EndTaskEvent(currentGameEvent);
        }
    }

}

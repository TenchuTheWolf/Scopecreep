using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    protected int currentTasksCompleted;
    protected bool taskReadyToSpawn;
    protected bool gameEventActive;
    protected int tasksToEndEvent;
    protected int tasksMissed;

    public List<TaskPickup> taskPrefabs = new List<TaskPickup>();

    protected virtual void Update()
    {
        if (currentTasksCompleted >= tasksToEndEvent && gameEventActive == true)
        {
            gameEventActive = false;
            EndEvent();
        }

        if (gameEventActive == true && taskReadyToSpawn == true)
        {
            GenerateTask();
        }
    }

    public virtual void GenerateTask()
    {
        taskReadyToSpawn = false;
        int taskAmountToSpawn = Random.Range(1, 3);
        bool moreThanOneTask = taskAmountToSpawn > 1;

        int randomIndex = Random.Range(0, taskPrefabs.Count);
        TaskPickup chosenTask = taskPrefabs[randomIndex];
        GameTaskManager.instance.SpawnTask(chosenTask, taskAmountToSpawn, moreThanOneTask);

        StartCoroutine(TaskAvailableTimer());
    }

    private IEnumerator TaskAvailableTimer()
    {
        yield return new WaitForSeconds(4);
        taskReadyToSpawn = true;
    }

    public void TaskCompleted()
    {
        currentTasksCompleted++;
    }

    public virtual void TaskMissed(TaskPickup pickupTarget)
    {
        tasksMissed++;
    }
    public virtual void EndEvent()
    {
        gameEventActive = false;
        currentTasksCompleted = 0;
        tasksMissed = 0;
        StopAllCoroutines();
        GameTaskManager.instance.EndTaskEvent(this);
    }
}

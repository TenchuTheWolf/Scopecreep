using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineEvent : GameEvent
{
    private float deadlineDuration;

    public NPC lowPriorityConsequence;
    public NPC mediumPriorityConsequence;
    public NPC highPriorityConsequence;

    private Dictionary<TaskPickup.TaskPriority, int> missedTasks = new Dictionary<TaskPickup.TaskPriority, int>();
    public void LaunchDeadlineEvent(int waveCount)
    {
        float dlDuration = 10 * waveCount;
        int dlTasks = 2 * waveCount;
        deadlineDuration = dlDuration;
        tasksToEndEvent = dlTasks;

        gameEventActive = true;
        taskReadyToSpawn = true;

        PanelManager.OpenPanel<BroadcastPanel>().ShowMessage("Deadline inbound! Get those tasks before your next milestone!");
        PanelManager.GetPanel<HUD>().CreateGoalEntry(GoalEntryTaskType.Deadline, "Make sure to collect all the tasks before your Deadline ends or you'll find a nasty surprise at the end.");

        AudioManager.SwapMusic(AudioTrack.Deadline);

        StartCoroutine(DeadlineEventTimer());
    }

    private IEnumerator DeadlineEventTimer()
    {
        yield return new WaitForSeconds(deadlineDuration);
        EndEvent();
    }

    public override void EndEvent()
    {
        if(missedTasks.Count > 0)
        {

            if (PanelManager.GetPanel<UpgradePanel>().IsOpen == true && gameEventActive == true)
            {
                Debug.LogError("Spawning enemies while the upgrade panel is open and the Deadline game event is active.");
            }

            foreach (var entry in missedTasks)
            {
                switch (entry.Key)
                {
                    case TaskPickup.TaskPriority.Low:
                        for (int i = 0; i < entry.Value; i++)
                        {
                            GameManager.gameManagerInstance.SpawnASingleEnemy(lowPriorityConsequence);
                        }
                        break;
                    case TaskPickup.TaskPriority.Medium:
                        for (int i = 0; i < entry.Value; i++)
                        {
                            GameManager.gameManagerInstance.SpawnASingleEnemy(mediumPriorityConsequence);
                        }
                        break;
                    case TaskPickup.TaskPriority.High:
                        for (int i = 0; i < entry.Value; i++)
                        {
                            GameManager.gameManagerInstance.SpawnASingleEnemy(highPriorityConsequence);
                        }
                        break;
                    default:
                        break;
                }
            }
            PanelManager.OpenPanel<BroadcastPanel>().ShowMessage("You missed your deadline, instead of funding, here's more bugs. :(");
            //spawn a shitload of adds multiplied by the number of missed tasks;
        }
        else
        {
            //generate a reward Design Docs += 2 * currentTasksCompleted;
            PanelManager.OpenPanel<BroadcastPanel>().ShowMessage("You met your deadline, here's some funding! :D");
        }

        AudioManager.SwapMusic(AudioTrack.Sprint);

        base.EndEvent();


    }

    public override void TaskMissed(TaskPickup pickupTarget)
    {
        if(missedTasks.ContainsKey(pickupTarget.myTaskPriority) == true)
        {
            missedTasks[pickupTarget.myTaskPriority]++;
        }
        else
        {
            missedTasks.Add(pickupTarget.myTaskPriority, 1);
        }

        base.TaskMissed(pickupTarget);
    }

    //GenerateDeadlineAnimation()
    //CleanUpDeadlineEvent
    //
    //Tasks/Animations/Visuals
}

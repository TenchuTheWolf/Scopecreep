using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrunchTimeEvent : GameEvent
{
    public List<NPC> consequenceNPCList = new List<NPC>();
    public void LaunchCrunchTimeEvent(int waveCount)
    {
        gameEventActive = true;
        taskReadyToSpawn = true;
        int taskTotal = 1 + (waveCount / 2);
        tasksToEndEvent = taskTotal;
        AudioManager.SwapMusic(AudioTrack.Crunchtime);
        PanelManager.OpenPanel<BroadcastPanel>().ShowMessage("It's Crunch Time! Pick up those tasks or perish.");
        PanelManager.GetPanel<HUD>().CreateGoalEntry(GoalEntryTaskType.Crunchtime, "The crunch is on! Make sure to not miss any tasks or you might stumble into more bugs along the way.");
    }

    public override void TaskMissed(TaskPickup pickupTarget)
    {
        base.TaskMissed(pickupTarget);

        int numberOfEnemies = Random.Range(1, 3 + tasksMissed);

        if(PanelManager.GetPanel<UpgradePanel>().IsOpen == true && gameEventActive == true)
        {
            Debug.LogError("Spawning enemies while the upgrade panel is open and the Crunchtime game event is active.");
            GameManager.gameManagerInstance.RemoveAllEnemies();
            return;
            //Potentially need to also destroy the recurring Micromanagers.
        }

        if (PanelManager.GetPanel<UpgradePanel>().IsOpen == true && gameEventActive == false)
        {
            Debug.LogError("Spawning enemies while the upgrade panel is open and the Crunchtime game event is false.");
        }

        if (PanelManager.GetPanel<UpgradePanel>().IsOpen == false)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                GameManager.gameManagerInstance.SpawnASingleEnemy(consequenceNPCList[Random.Range(0, consequenceNPCList.Count)]);
            }
        }


    }


    public override void EndEvent()
    {
        base.EndEvent();
        PanelManager.OpenPanel<BroadcastPanel>().ShowMessage("You survived the Crunch Time!");
        AudioManager.SwapMusic(AudioTrack.Sprint);
    }
}

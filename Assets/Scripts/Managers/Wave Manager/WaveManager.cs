using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();

    public int nextWave;
    public void SpawnWave(int waveIndex, bool forceSpawn = false)
    {
        //This is for the upgrade panel
        if(waveIndex % 1 == 0 && forceSpawn == false && waveIndex != 0)
        {
            PanelManager.GetPanel<HUD>().Close();
            PanelManager.OpenPanel<UpgradePanel>();
            return;
        }
        if(waveIndex >= waves.Count && PlayerPrefs.GetInt("Dev Hell") == 0)
        {
            PanelManager.OpenPanel<EndGamePanel>().Setup(false);
            Destroy(Player.playerInstance.gameObject);
            return;
        }
        if(waveIndex >= waves.Count && PlayerPrefs.GetInt("Dev Hell") == 1)
        {
            if(DevHellMode.devHellModeActive == false)
            {
                PanelManager.OpenPanel<BroadcastPanel>().ShowColorMessage("Removing Work/Life Balance Limitations...", Color.red, 3f);
                PanelManager.GetPanel<HUD>().CreateGoalEntry(GoalEntryTaskType.DevHell, "Perish.");
                DevHellMode.devHellModeActive = true;
                DevHellMode.instance.SpawnEnemies();
                AudioManager.SwapMusic(AudioTrack.DevHell);
            }
            return;
        }

        waves[waveIndex].SpawnWave();
        nextWave++;
        PanelManager.GetPanel<HUD>().CreateGoalEntry(GoalEntryTaskType.Sprint, "Survive the sprint, or else. :D");

    }

    public void ResetWaveCount()
    {
        nextWave = 0;
    }



}

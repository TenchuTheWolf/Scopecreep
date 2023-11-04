using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameKillLog : MonoBehaviour
{
    public EnemyCountEntry counterTemplate;
    public Transform entryHolder;
    private List<EnemyCountEntry> killEntries = new List<EnemyCountEntry>();

    public void SetupCounterEntry()
    {
        ClearCounterEntries();
        foreach (KeyValuePair<NPC.EnemyType, List<DogTagData>> killListEntry in GameManager.gameManagerInstance.killList)
        {
            EnemyCountEntry activeEntry = Instantiate(counterTemplate, entryHolder);
            activeEntry.gameObject.SetActive(true);
            activeEntry.SetupCounter(killListEntry.Value[0], killListEntry.Value.Count);
            killEntries.Add(activeEntry);
        }
    }

    public void ClearCounterEntries()
    {
        for (int i = 0; i < killEntries.Count; i++)
        {
            Destroy(killEntries[i].gameObject);
        }
        killEntries.Clear();
    }
}

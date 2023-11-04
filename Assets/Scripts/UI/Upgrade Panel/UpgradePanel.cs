using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UpgradePanel : BasePanel
{
    public UpgradePanelEntry buttonTemplate;
    public Transform buttonHolder;

    public TextMeshProUGUI currencyText;

    private List<UpgradePanelEntry> entries = new List<UpgradePanelEntry>();

    private Dictionary<int, UpgradeIDNumber> upgradePositionDict = new Dictionary<int, UpgradeIDNumber>();

    public override void Open()
    {
        base.Open();
        //AudioManager.SwapMusic(AudioTrack.MainMenu);
        Setup();
    }

    protected override void Awake()
    {
        base.Awake();
        buttonTemplate.gameObject.SetActive(false);
    }

    public void Setup()
    {
        ClearList();
        for (int i = 0; i < UpgradeManager.instance.CurrentUpgradeDataPool.Count; i++)
        {
            CreateButton(UpgradeManager.instance.CurrentUpgradeDataPool[i]);
        }
    }

    private void ClearList()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            Destroy(entries[i].gameObject);
        }
        entries.Clear();
    }
    private void CreateButton(UpgradeData targetData)
    {
        UpgradePanelEntry newButton = Instantiate(buttonTemplate, buttonHolder);
        newButton.SetupButton(targetData, this);
        newButton.gameObject.SetActive(true);
        newButton.gameObject.name = targetData.upgradeID.ToString();
        entries.Add(newButton);
    }

    private void SaveUpgradeIndices(Transform excludedButton)
    {
        upgradePositionDict.Clear();

        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].transform == excludedButton)
            {
                //Debug.Log("This is the index of the upgrade we want to replace: " + GetUpgradeIndex(excludedButton));
                upgradePositionDict.Add(GetUpgradeIndex(excludedButton), UpgradeIDNumber.None);
            }
            else
            {

                int savedIndex = GetUpgradeIndex(entries[i].transform);
                upgradePositionDict.Add(savedIndex, entries[i].EntryUpgradeData.upgradeID);
            }
        }

    }

    public void OnUpgradePurchased(UpgradeData purchasedUpgrade, Transform buttonThatWasClicked)
    {
        int purchasedButtonIndex = -1;
        UpgradeIDNumber targetUpgradeID = UpgradeIDNumber.None;


        if (purchasedUpgrade.upgradesToAdd.Count == 1)
        {
            purchasedButtonIndex = GetUpgradeIndex(buttonThatWasClicked);
            targetUpgradeID = purchasedUpgrade.upgradesToAdd[0];
        }



        for (int i = 0; i < purchasedUpgrade.upgradesToAdd.Count; i++)
        {
            UpgradeManager.AddUpgradeToPool(purchasedUpgrade.upgradesToAdd[i]);
        }

        for (int i = 0; i < purchasedUpgrade.upgradesToRemove.Count; i++)
        {
            UpgradeManager.RemoveUpgradeFromPool(purchasedUpgrade.upgradesToRemove[i]);
        }


        SaveUpgradeIndices(buttonThatWasClicked);
        Setup();

        if (purchasedButtonIndex > -1)
        {

            StartCoroutine(updateButtonPositions(purchasedButtonIndex, targetUpgradeID, buttonThatWasClicked));
        }
    }

    private IEnumerator updateButtonPositions(int targetIndex, UpgradeIDNumber targetID, Transform buttonThatWasClicked)
    {
        yield return new WaitForEndOfFrame();

        foreach (var entry in upgradePositionDict)
        {
            if (entry.Value == UpgradeIDNumber.None)
            {
                Transform targetButton = GetUpgradeTransformByID(targetID);
                if (targetButton != null)
                {
                    //Debug.Log("Setting " + targetID + " to position " + entry.Key);
                    SetUpgradeIndex(entry.Key, targetButton);
                }
            }
            else
            {
                SetUpgradeIndex(entry.Key, GetUpgradeTransformByID(entry.Value));
            }
        }


    }

    public UpgradePanelEntry GetEntryByData(UpgradeData data)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].EntryUpgradeData == data)
            {
                return entries[i];
            }
        }
        return null;
    }

    public void OnContinueClicked()
    {
        PanelManager.GetPanel<HUD>().Open();
        GameManager.gameManagerInstance.waveManager.SpawnWave(GameManager.gameManagerInstance.waveManager.nextWave, true);
        Player.playerInstance.transform.position = Vector3.zero;
        Close();
    }

    public void UpdateCurrencyDisplay(int playerCurrentCurrency)
    {
        currencyText.text = playerCurrentCurrency.ToString();
    }

    public void OnMyUpgradesClicked()
    {
        PanelManager.OpenPanel<UpgradeCollectionPanel>();
    }

    public override void Close()
    {
        base.Close();
        //Put an if statement checking if DevHell is active if there is a conflict
        //AudioManager.SwapMusic(AudioTrack.Sprint);
    }


    /// <summary>
    /// Returns the index of an Upgrade Panel Entry in a hierarchy under its parent.
    /// </summary>
    public int GetUpgradeIndex(Transform targetButtonTransform)
    {
        int precursorIndex = targetButtonTransform.GetSiblingIndex();
        return precursorIndex;
    }


    /// <summary>
    /// Sets the index of an Upgrade Panel Entry in a hierarchy under its parent.
    /// </summary>
    public void SetUpgradeIndex(int targetIndex, Transform targetButtonTransform)
    {
        targetButtonTransform.SetSiblingIndex(targetIndex);
    }


    /// <summary>
    /// Returns the existing transform of an active button based on the Upgrade ID provided.
    /// </summary>
    /// <param name="targetUpgradeID"></param>
    /// <returns></returns>
    public Transform GetUpgradeTransformByID(UpgradeIDNumber targetUpgradeID)
    {

        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].EntryUpgradeData.upgradeID == targetUpgradeID)
            {
                return entries[i].transform;
            }
        }
        return null;
    }

}

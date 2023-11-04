using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeCollectionPanel : BasePanel, IPointerEnterHandler
{

    public UpgradePanelEntry buttonTemplate;
    public Transform buttonHolder;

    private List<UpgradePanelEntry> entries = new List<UpgradePanelEntry>();

    protected override void Awake()
    {
        base.Awake();
        buttonTemplate.gameObject.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
        Setup();
    }

    public override void Close()
    {
        base.Close();
        TooltipSystem.Hide();
    }

    public void Setup()
    {
        ClearList();
        for (int i = 0; i < UpgradeManager.instance.purchasedUpgrades.Count; i++)
        {
            CreateButton(UpgradeManager.instance.purchasedUpgrades[i]);
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
        newButton.SetupButton(targetData);
        newButton.gameObject.SetActive(true);
        entries.Add(newButton);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
}

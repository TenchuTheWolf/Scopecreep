using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradePanelEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //UnityEngine.EventSystems and IPointerEnterHandler + IPointerExitHandler are used for on mouse over events.

    private UpgradePanel parentPanel;

    public Image upgradeIcon;
    public TextMeshProUGUI upgradeCostField;
    public TextMeshProUGUI upgradeNameField;
    public UpgradeData EntryUpgradeData { get; private set; }

    public void SetupButton(UpgradeData upgradeData, UpgradePanel parentPanel = null)
    {
        this.EntryUpgradeData = upgradeData;
        this.parentPanel = parentPanel;

        upgradeIcon.sprite = upgradeData.upgradeIcon;

        if(upgradeNameField != null)
        {
            upgradeNameField.text = upgradeData.upgradeName.ToString();
        }
        
        if(upgradeCostField != null)
        {
            if (upgradeData.upgradeCost == 0)
            {
                upgradeCostField.text = "Free";
            }
            else
            {
                upgradeCostField.text = upgradeData.upgradeCost.ToString();
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(EntryUpgradeData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    public void OnPurchaseClicked()
    {
        if(Player.playerInstance.TrySpendCurrency(EntryUpgradeData.upgradeCost) == false)
        {
            Debug.LogWarning("You don't have enough design docs to access this new feature.");
            return;
        }
        UpgradeManager.OnUpgradeSelected(EntryUpgradeData);
        parentPanel.OnUpgradePurchased(EntryUpgradeData, transform);
        TooltipSystem.Hide();
    }


}

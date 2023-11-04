using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HUD : BasePanel
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI healthText;
    public Image regularHealthBarFill;
    public bool gameStarted;

    [Header("Goal Entry Stuff")]
    public SprintGoalEntry goalEntryTemplate;
    public Transform goalEntryHolder;
    private List<SprintGoalEntry> currentGoals = new List<SprintGoalEntry>();
    
    protected override void Awake()
    {
        base.Awake();
        goalEntryTemplate.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && gameStarted == false)
        {
            gameStarted = true;
            GameManager.gameManagerInstance.StartGame();
        }
    }
    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void UpdateCurrencyDisplay(int playerCurrentCurrency)
    {
        currencyText.text = playerCurrentCurrency.ToString();
    }

    public void UpdatePlayerUIHealthBar(float healthPercentage)
    {
        regularHealthBarFill.fillAmount = healthPercentage;
        healthText.text = Player.playerInstance.PlayerHealth.currentHealth + "/" + Player.playerInstance.PlayerHealth.maxHealth;
    }

    #region GOAL ENTRIES

    public void CreateGoalEntry(GoalEntryTaskType targetGoal, string goalText)
    {
        ClearGoalEntries();
        SprintGoalEntry newEntry = Instantiate(goalEntryTemplate, goalEntryHolder);
        newEntry.gameObject.SetActive(true);
        newEntry.Setup(goalText, targetGoal);
        newEntry.FadeGoalTextOn();
        currentGoals.Add(newEntry);
    }

    private void ClearGoalEntries()
    {
        foreach (var goal in currentGoals)
        {
            goal.FadeCheckBoxOn();
            goal.FadeGoalTextOff();
        }
        currentGoals.Clear();
    }

    #endregion 


}

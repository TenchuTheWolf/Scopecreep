using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class EndGamePanel : BasePanel
{

    [Header("Text Fields")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI finalScoreText;
    public EndGameKillLog killLog;

    [Header("Buttons")]
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Attract Mode List")]
    public List<Asteroid> attractModeList = new List<Asteroid>();


    protected override void Awake()
    {
        base.Awake();
    }

    public override void Open()
    {
        base.Open();
        restartButton.interactable = false;
        mainMenuButton.interactable = false;
        FadeIn(EnableButton);
    }

    private void EnableButton()
    {
        restartButton.interactable = true;
        mainMenuButton.interactable = true;
    }

    public void Setup(bool playerDied)
    {

        if(playerDied == true)
        {
            titleText.text = "Oops";
        }
        else
        {
            titleText.text = "You Won!";
        }

        killLog.SetupCounterEntry();
        finalScoreText.text = "Final Score: " + Player.playerInstance.playerScore.ToString();
    }

    public void OnRestartClicked()
    {
        restartButton.interactable = false;
        killLog.ClearCounterEntries();
        GameManager.gameManagerInstance.StartGame();
        PanelManager.GetPanel<HUD>().SetScoreText("0");
        FadeOut(Close);
        AudioManager.SwapMusic(AudioTrack.Sprint);
    }

    public void OnMainMenuClicked()
    {
        

        mainMenuButton.interactable = false;
        PanelManager.GetPanel<HUD>().Close();
        killLog.ClearCounterEntries();
        PanelManager.GetPanel<HUD>().SetScoreText("0");
        FadeOut(Close);
        AudioManager.SwapMusic(AudioTrack.MainMenu);
        PanelManager.OpenPanel<MainMenuPanel>().Open();
        //Need to remove healthbars before removing enemies or they get stuck.
        GameManager.gameManagerInstance.CleanUpHealthbars();
        GameManager.gameManagerInstance.RemoveAllEnemies();

        for (int i = 0; i < attractModeList.Count; i++)
        {
            GameManager.gameManagerInstance.SpawnASingleEnemy(attractModeList[i]);
        }
        GameManager.gameManagerInstance.CleanUpHealthbars();
        //Need to remove healthbars after repopulating the attractor mode enemies on the main screen.
    }

}

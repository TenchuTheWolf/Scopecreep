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

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Open()
    {
        base.Open();
        restartButton.interactable = false;
        FadeIn(EnableButton);
    }

    private void EnableButton()
    {
        restartButton.interactable = true;
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

}

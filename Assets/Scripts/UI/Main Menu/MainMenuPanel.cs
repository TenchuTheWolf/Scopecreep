using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : BasePanel
{
    public override void Open()
    {
        base.Open();
        AudioManager.SwapMusic(AudioTrack.MainMenu);
    }


    public void OnStartGameClicked()
    {
        GameManager.gameManagerInstance.StartGame();
        Close();
        AudioManager.SwapMusic(AudioTrack.Sprint);
    }

    public void OnOptionsClicked()
    {
        PanelManager.OpenPanel<OptionsPanel>();
    }

    public void OnExitGameClicked()
    {
        Application.Quit();
    }
}

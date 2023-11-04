using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class OptionsPanel : BasePanel
{

    public Slider masterVolume;
    public Toggle devHellToggle;
    public Toggle tutorialsToggle;
    public TMP_Dropdown resDropdown;

    public Texture2D colorChart;
    public GameObject chartObject;
    public Image chartImage;
    public ColorChartHelper colorChartHelper;

    public Color outputColor;

    public Image defaultCursorSwatch;
    public Image enemyMouseoverCursorSwatch;
    public bool defaultSwatchSelected;

    [HideInInspector]
    public Image selectedSwatch;

    private void Start()
    {

        int defaultResHeight = PlayerPrefs.GetInt("Default Resolution Height");
        int defaultResWidth = PlayerPrefs.GetInt("Default Resolution Width");
        
        if(defaultResHeight == 0 && defaultResWidth == 0)
        {
            Screen.SetResolution(1920, 1080, false);
            PlayerPrefs.SetInt("Default Resolution Height", 1080);
            PlayerPrefs.SetInt("Default Resolution Width", 1920);
        }

        Color defaultLaunchCursorColor;
        float defaultRed = PlayerPrefs.GetFloat("Default Cursor Red Value");
        float defaultGreen = PlayerPrefs.GetFloat("Default Cursor Green Value");
        float defaultBlue = PlayerPrefs.GetFloat("Default Cursor Blue Value");

        if(defaultRed == 0 && defaultGreen == 0 && defaultBlue == 0)
        {
            defaultLaunchCursorColor = Color.white;
        }
        else
        {
            defaultLaunchCursorColor = new Color(defaultRed, defaultGreen, defaultBlue, 1f);
        }

        CursorHelper.instance.OnDefaultColorChange(defaultLaunchCursorColor);
        defaultCursorSwatch.color = defaultLaunchCursorColor;

        Color enemyLaunchCursorColor;
        float defaultEnemyRed = PlayerPrefs.GetFloat("Enemy Cursor Red Value");
        float defaultEnemyGreen = PlayerPrefs.GetFloat("Enemy Cursor Green Value");
        float defaultEnemyBlue = PlayerPrefs.GetFloat("Enemy Cursor Blue Value");

        if (defaultEnemyRed == 0 && defaultEnemyGreen == 0 && defaultEnemyBlue == 0)
        {
            enemyLaunchCursorColor = Color.red;
        }
        else
        {
            enemyLaunchCursorColor = new Color(defaultEnemyRed, defaultEnemyGreen, defaultEnemyBlue, 1f);
        }

        CursorHelper.instance.OnEnemyColorChange(enemyLaunchCursorColor);
        enemyMouseoverCursorSwatch.color = enemyLaunchCursorColor;

        AudioManager.UpdateMusicLevels();
    }

    public void OnDefaultColorSwatchClicked()
    {
        selectedSwatch = defaultCursorSwatch;
        defaultSwatchSelected = true;
    }

    public void OnEnemyMouseoverColorSwatchClicked()
    {
        selectedSwatch = enemyMouseoverCursorSwatch;
        defaultSwatchSelected = false;
    }

    public void OnColorChosen(Color targetColor)
    {
        selectedSwatch.color = targetColor;
        if(defaultSwatchSelected == true)
        {
            CursorHelper.instance.OnDefaultColorChange(targetColor);
            PlayerPrefs.SetFloat("Default Cursor Red Value", targetColor.r);
            PlayerPrefs.SetFloat("Default Cursor Green Value", targetColor.g);
            PlayerPrefs.SetFloat("Default Cursor Blue Value", targetColor.b);
        }
        else
        {
            CursorHelper.instance.OnEnemyColorChange(targetColor);
            PlayerPrefs.SetFloat("Enemy Cursor Red Value", targetColor.r);
            PlayerPrefs.SetFloat("Enemy Cursor Green Value", targetColor.g);
            PlayerPrefs.SetFloat("Enemy Cursor Blue Value", targetColor.b);
        }
    }


    public override void Open()
    {
        base.Open();
        colorChartHelper.Setup(chartImage, this);

        int devHell = PlayerPrefs.GetInt("Dev Hell", 0);
        devHellToggle.isOn = devHell == 1 ? true : false;

        int tutorialsEnabled = PlayerPrefs.GetInt("Tutorials Enabled", 0);
        tutorialsToggle.isOn = tutorialsEnabled == 1 ? false : true;

        masterVolume.value = PlayerPrefs.GetFloat("Master Volume", 1f);
    }

    public void DevHellModeToggled()
    {

        if (devHellToggle.isOn == true)
        {
            PlayerPrefs.SetInt("Dev Hell", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Dev Hell", 0);
        }

    }

    public void TutorialsToggled()
    {

        if (tutorialsToggle.isOn == true)
        {
            PlayerPrefs.SetInt("Tutorials Enabled", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Tutorials Enabled", 0);
        }

    }



    public void OnMasterVolumeChanged()
    {
        PlayerPrefs.SetFloat("Master Volume", masterVolume.value);
        AudioManager.UpdateMusicLevels();
    }

    public void OnResolutionChanged()
    {
        switch (resDropdown.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, false);
                PlayerPrefs.SetInt("Default Resolution Height", 1080);
                PlayerPrefs.SetInt("Default Resolution Width", 1920);
                break;
            case 1:
                Screen.SetResolution(1536, 864, false);
                PlayerPrefs.SetInt("Default Resolution Height", 864);
                PlayerPrefs.SetInt("Default Resolution Width", 1536);
                break;
            case 2:
                Screen.SetResolution(1366, 768, false);
                PlayerPrefs.SetInt("Default Resolution Height", 768);
                PlayerPrefs.SetInt("Default Resolution Width", 1366);
                break;
            case 3:
                Screen.SetResolution(1280, 720, false);
                PlayerPrefs.SetInt("Default Resolution Height", 720);
                PlayerPrefs.SetInt("Default Resolution Width", 1280);
                break;
        }


        Debug.Log("You picked: " + resDropdown.value);

    }


}

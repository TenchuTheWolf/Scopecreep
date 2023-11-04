using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : BasePanel
{

    public static PausePanel instance;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true && PausePanel.instance.isActiveAndEnabled == false)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    

}

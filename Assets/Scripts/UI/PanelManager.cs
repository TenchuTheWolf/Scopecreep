using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanelManager : MonoBehaviour
{
    public List<BasePanel> panelList = new List<BasePanel>();
    public static PanelManager instance;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        panelList = GetComponentsInChildren<BasePanel>(true).ToList();
    }

    //T is a 'generic' that allows this method to constrain what it takes to anything that inherits from BasePanel.
    public static bool IsPanelOpen<T>() where T : BasePanel
    {
        T targetPanel = GetPanel<T>();

        if (targetPanel != null)
        {
            return targetPanel.IsOpen;
        }

        return false;

    }

    public static T OpenPanel<T>() where T : BasePanel
    {
        T targetPanel = GetPanel<T>();

        if(targetPanel != null)
        {
            targetPanel.Open();
            return targetPanel;
        }

        Debug.LogError("Could not find panel of type: " + typeof(T).ToString());
        return null;


    }

    public static T GetPanel<T>() where T : BasePanel
    {
        for (int i = 0; i < instance.panelList.Count; i++)
        {
            if (instance.panelList[i] is T)
            {
                return instance.panelList[i] as T;
            }
        }
        Debug.LogError("Could not find panel of type: " + typeof(T).ToString());
        return null;
    }

}

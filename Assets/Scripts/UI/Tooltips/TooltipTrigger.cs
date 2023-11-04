using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string uiElementName;
    public string uiElementDescription;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(uiElementName, uiElementDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}

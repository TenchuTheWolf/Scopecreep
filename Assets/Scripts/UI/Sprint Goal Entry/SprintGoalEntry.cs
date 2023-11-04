using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GoalEntryTaskType
{
    Sprint,
    Crunchtime,
    Deadline,
    DevHell
}

public class SprintGoalEntry : MonoBehaviour
{

    public TextMeshProUGUI goalText;
    public GameObject checkboxFill;
    public CanvasGroup checkboxCanvasGroup;
    public CanvasGroup entryCanvasGroup;
    public GoalEntryTaskType goalEntryTaskType;
    public LayoutElement myLayoutElement;

    public void Setup(string setText, GoalEntryTaskType targetTaskType)
    {
        goalText.text = setText;
        goalEntryTaskType = targetTaskType;
        TurnCheckBoxOff();
        StartCoroutine(AdjustTextBoxSize());
    }

    public IEnumerator AdjustTextBoxSize()
    {
        yield return new WaitForEndOfFrame();
        float textBoxHeight = goalText.rectTransform.sizeDelta.y;
        myLayoutElement.preferredHeight = textBoxHeight;
    }

    public void FadeCheckBoxOn()
    {
        UIUtilities.FadeIn(checkboxCanvasGroup, 5f);
    }

    public void FadeGoalTextOn()
    {
        UIUtilities.FadeIn(entryCanvasGroup);
    }

    public void FadeGoalTextOff()
    {
        UIUtilities.FadeOut(entryCanvasGroup, callback:SelfDestructGoalEntry);
    }

    public void TurnCheckBoxOff()
    {
        UIUtilities.FadeOut(checkboxCanvasGroup, 5f);
    }

    public void SelfDestructGoalEntry()
    {
        Destroy(gameObject);
    }

}

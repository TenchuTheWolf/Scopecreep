using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuTextCycler : MonoBehaviour
{
    [Header("Commentary Description Options")]
    public float textCycleDelay = 6f;
    public float textParseDelay = .05f;

    [Header ("Comment Box Options")]
    public List<string> mainMenuCommentary =  new List<string>();
    private List<string> usedCommentary = new List<string>();
    private TextMeshProUGUI targetTextBox;
    private string newDialogueChoice;

    private void Awake()
    {
        usedCommentary.Clear();
        targetTextBox = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ParseText(mainMenuCommentary[0], textParseDelay));
        usedCommentary.Add(mainMenuCommentary[0]);
    }

    private void Start()
    {
        StartCoroutine(TextUpdateTimer());
    }

    private void UpdateDialogue()
    {

        if(PanelManager.GetPanel<MainMenuPanel>().IsOpen == true)
        {

            if(usedCommentary.Count == mainMenuCommentary.Count)
            {
                string lastComment = usedCommentary[usedCommentary.Count - 1];
                usedCommentary.Clear();
                usedCommentary.Add(lastComment);
            }

            for (int i = 0; i < mainMenuCommentary.Count; i++)
            {
                int randomIndex = Random.Range(0, mainMenuCommentary.Count);
                if(usedCommentary.Contains(mainMenuCommentary[randomIndex]) == false)
                {
                    newDialogueChoice = mainMenuCommentary[randomIndex];
                    usedCommentary.Add(mainMenuCommentary[randomIndex]);
                    StartCoroutine(ParseText(newDialogueChoice, textParseDelay));
                    break;
                }
            }

            StartCoroutine(TextUpdateTimer());
        }

    }

    private IEnumerator TextUpdateTimer()
    {
        yield return new WaitForSeconds(textCycleDelay);
        UpdateDialogue();
    }

    private IEnumerator ParseText(string textToParse, float parseDelay)
    {
        string fullText = textToParse;

        for (int i = 0; i < fullText.Length; i++)
        {
            targetTextBox.text = fullText.Substring(0, i + 1);
            yield return new WaitForSeconds(parseDelay);
        }
    }


}

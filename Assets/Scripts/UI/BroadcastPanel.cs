using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BroadcastPanel : BasePanel
{
    public TextMeshProUGUI broadcastText;

    public void ShowMessage(string message, float time = 3f)
    {
        StartCoroutine(ShowMessageCoroutine(message, time));
    }

    public void ShowColorMessage(string message, Color textColor, float time = 3f)
    {
        StartCoroutine(ShowColorMessageCoroutine(message, textColor, time));
    }

    private IEnumerator ShowMessageCoroutine(string message, float time = 3f) 
    {
        broadcastText.text = message;
        broadcastText.color = Color.white;
        yield return new WaitForSeconds(time);
        broadcastText.text = string.Empty;
    }

    private IEnumerator ShowColorMessageCoroutine(string message, Color color, float time = 3f)
    {
        broadcastText.text = message;
        broadcastText.color = color;
        yield return new WaitForSeconds(time);
        broadcastText.text = string.Empty;
    }



}

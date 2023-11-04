using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIUtilities : MonoBehaviour
{

    public static UIUtilities instance;

    private void Awake()
    {
        instance = this;
    }

    public static void FadeIn(CanvasGroup targetCanvas, float fadeModifier = 1f, Action callback = null)
    {
        targetCanvas.alpha = 0f;
        instance.StartCoroutine(Fade(targetCanvas, 1, fadeModifier, callback));
    }


    public static void FadeOut(CanvasGroup targetCanvas, float fadeModifier = 1f, Action callback = null)
    {
        targetCanvas.alpha = 1f;
        instance.StartCoroutine(Fade(targetCanvas, 0, fadeModifier, callback));
    }

    public static IEnumerator Fade(CanvasGroup targetCanvas, float targetValue, float fadeModifier = 1f, Action callback = null)
    {
        if (targetCanvas == null)
        {
            callback?.Invoke();

            yield break;
        }

        while (targetCanvas.alpha != targetValue)
        {
            targetCanvas.alpha = Mathf.MoveTowards(targetCanvas.alpha, targetValue, Time.deltaTime * fadeModifier);
            yield return new WaitForEndOfFrame();
        }

        callback?.Invoke();
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    public Tooltip tooltip;

    public void Awake()
    {
        instance = this;
    }

    public static void Show(UpgradeData data)
    {
        instance.tooltip.gameObject.SetActive(true);
        instance.tooltip.SetString(data);
        instance.FadeIn();
    }

    public static void Show(string header, string body)
    {
        instance.tooltip.gameObject.SetActive(true);
        instance.tooltip.SetString(body, header);
        instance.FadeIn();
    }

    public static void Hide()
    {
        instance.FadeOut();
        instance.tooltip.gameObject.SetActive(false);
    }

    public void FadeIn(Action callback = null)
    {
        instance.tooltip.tooltipCanvasGroup.alpha = 0f;
        StartCoroutine(Fade(1));
    }

    public void FadeOut(Action callback = null)
    {
        instance.tooltip.tooltipCanvasGroup.alpha = 0f;
        StartCoroutine(Fade(0));
    }

    public IEnumerator Fade(float targetFadeValue, Action callback = null)
    {
        if(instance.tooltip.tooltipCanvasGroup == null)
        {
            callback?.Invoke();

            yield break;
        }

        while(instance.tooltip.tooltipCanvasGroup.alpha != targetFadeValue)
        {
            instance.tooltip.tooltipCanvasGroup.alpha = Mathf.MoveTowards(instance.tooltip.tooltipCanvasGroup.alpha, targetFadeValue, Time.deltaTime * instance.tooltip.fadeModifier);
            yield return new WaitForEndOfFrame();
        }

        callback?.Invoke();
    }

}

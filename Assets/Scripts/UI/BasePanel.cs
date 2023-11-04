using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected GameObject view;

    public bool showMenuCursor;
    public bool IsOpen { get; protected set; }

    [Header("Fade Options")]
    public float fadeModifier = 1f;

    [Header("Buttons")]
    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        view = transform.Find("View").gameObject;

        if(view == null)
        {
            Debug.LogError("Could not find a View for a panel: "
               + GetType().Name
               + ". Ensure that all viewable parts of the panel are under a View Gameobject");
        }

        if(view.activeInHierarchy == true)
        {
            IsOpen = true;
        }

        canvasGroup = GetComponentInChildren<CanvasGroup>(true);
    }

    public void FadeIn(Action callback = null)
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(Fade(1, callback));
    }


    public void FadeOut(Action callback = null)
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(Fade(0, callback));
    }

    public IEnumerator Fade(float targetValue, Action callback = null)
    {
        if(canvasGroup == null)
        {
            callback?.Invoke();

            yield break;
        }

        while(canvasGroup.alpha != targetValue)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetValue, Time.deltaTime * fadeModifier);
            yield return new WaitForEndOfFrame();
        }

        callback?.Invoke();
    }

    public virtual void Open()
    {
        view.SetActive(true);
        IsOpen = true;

        if (showMenuCursor == true)
        {
            CursorHelper.instance.TurnUICursorOn();
        }

    }

    public virtual void Close()
    {
        view.SetActive(false);
        IsOpen = false;

        if (showMenuCursor == true)
        {
            CursorHelper.instance.TurnFiringReticuleOn();
        }
    }




}

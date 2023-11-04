using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [Header("Fade Options")]
    public float fadeModifier = 1f;


    public TextMeshProUGUI tooltipHeader;
    public TextMeshProUGUI tooltipDescription;

    public RectTransform tooltipTransform;

    public LayoutElement layoutElement;
    public CanvasGroup tooltipCanvasGroup;
    public int characterWrapLimit;

    public void Awake()
    {
        tooltipTransform = GetComponent<RectTransform>();
        tooltipCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetString(string description, string header = " ")
    {
        if (string.IsNullOrEmpty(header))
        {
            tooltipHeader.gameObject.SetActive(false);
        }
        else
        {
            tooltipHeader.gameObject.SetActive(true);
            tooltipHeader.text = header;
        }

        tooltipDescription.text = description;

    }

    public void SetString(UpgradeData data)
    {
        tooltipHeader.gameObject.SetActive(true);
        tooltipHeader.text = data.upgradeName;
        tooltipDescription.text = data.upgradeDescription;
    }


    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = tooltipHeader.text.Length;
            int descriptionLength = tooltipDescription.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || descriptionLength > characterWrapLimit) ? true : false;
        }

        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        tooltipTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;

    }

}

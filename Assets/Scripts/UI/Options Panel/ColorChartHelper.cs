using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChartHelper : MonoBehaviour, IPointerClickHandler
{

    private Image chartImage;
    private OptionsPanel optionsPanel;

    public void Setup(Image chartImage, OptionsPanel optionsPanel)
    {
        this.chartImage = chartImage;
        this.optionsPanel = optionsPanel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Color outputColor = Pick(chartImage);
        optionsPanel.OnColorChosen(outputColor);
        transform.parent.gameObject.SetActive(false);
    }

    private Color Pick(Image imageToPick)
    {
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageToPick.rectTransform, Input.mousePosition, Camera.main, out point);
        point += imageToPick.rectTransform.sizeDelta / 2;
        Texture2D texture = GetComponent<Image>().sprite.texture;
        Vector2Int mousePoint = new Vector2Int((int)((texture.width * point.x) / imageToPick.rectTransform.sizeDelta.x), (int)((texture.height * point.y) / imageToPick.rectTransform.sizeDelta.y));
        return texture.GetPixel(mousePoint.x, mousePoint.y);
    }
}

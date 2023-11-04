using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyCountEntry : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public Image enemyImage;

    public void SetupCounter(DogTagData enemyDogTag, int killTotal)
    {
        enemyImage.sprite = enemyDogTag.enemySprite;
        counterText.text = killTotal.ToString();
    }
}

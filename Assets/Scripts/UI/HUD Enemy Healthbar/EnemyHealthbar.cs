using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    public Vector2 healthbarOffset;
    public Transform followTarget;
    public Image enemyHealthbarFill;
    public float healthbarSizeScalar;

    private void Update()
    {
        if (followTarget != null)
        {
            Vector2 convertedPosition = Camera.main.WorldToScreenPoint(followTarget.transform.position);
            transform.position = convertedPosition + healthbarOffset;
        }
    }


    public void SetupHealthbar(NPC targetNPC, Transform targetTransform)
    {
        followTarget = targetTransform;
        healthbarSizeScalar = targetNPC.enemyhealthbarScale;
        healthbarOffset = targetNPC.enemyhealthbarOffset;

        transform.localScale *= healthbarSizeScalar;
    }

    public void UpdateHealthbar(float healthChangeValue)
    {
        enemyHealthbarFill.fillAmount = healthChangeValue;
    }

}

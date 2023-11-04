using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtilities
{
    public static IEnumerator FadeSprite(float targetValue, SpriteRenderer renderer, float fadeDuration)
    {
        if (renderer == null)
        {
            Debug.LogError("The array did not provide a sprite renderer.");
            yield break;
        }

        while (renderer.color.a != targetValue)
        {

            float desiredAlpha = Mathf.MoveTowards(renderer.color.a, targetValue, Time.deltaTime * (1 / fadeDuration));
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, desiredAlpha);

            yield return new WaitForEndOfFrame();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHelper : MonoBehaviour
{
    public static CursorHelper instance;
    public float cursorSizeMinimum;
    public float cursorAnimationSpeed = 1f;
    public Color defaultCursorColor;
    public Color enemyMouseOverColor;
    private Vector3 defaultCursorSize;
    private bool reticuleIsSqueezing;
    private bool reticuleIsReleasing;
    private SpriteRenderer[] spriteRenderersArray;

    private void Awake()
    {
        defaultCursorSize = transform.localScale;
        spriteRenderersArray = GetComponentsInChildren<SpriteRenderer>();
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(SqueezeReticule());
        }
        FollowCursor();
    }

    private void FollowCursor()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }

    public void OnDefaultColorChange(Color targetColor)
    {
        defaultCursorColor = targetColor;
        ChangeCursorColor(targetColor);
    }

    public void OnEnemyColorChange(Color targetColor)
    {
        enemyMouseOverColor = targetColor;
        ChangeCursorColor(targetColor);

    }

    public void TurnFiringReticuleOn()
    {
        for (int i = 0; i < spriteRenderersArray.Length; i++)
        {
            spriteRenderersArray[i].gameObject.SetActive(true);
        }

        Cursor.visible = false;
    }

    public void TurnUICursorOn()
    {
        for (int i = 0; i < spriteRenderersArray.Length; i++)
        {
            spriteRenderersArray[i].gameObject.SetActive(false);
        }

        Cursor.visible = true;

    }


    public IEnumerator SqueezeReticule()
    {
        if(reticuleIsReleasing == true || reticuleIsSqueezing == true)
        {
            yield break;
        }

        reticuleIsSqueezing = true;

        while (transform.localScale.x > cursorSizeMinimum)
        {
            float desiredCrosshairSize = Mathf.MoveTowards(transform.localScale.x, cursorSizeMinimum, Time.deltaTime * cursorAnimationSpeed);
            transform.localScale = new Vector3(desiredCrosshairSize, desiredCrosshairSize, desiredCrosshairSize); 
            yield return new WaitForEndOfFrame();
        }
        reticuleIsSqueezing = false;
        StartCoroutine(ResetCrosshairSize());
    }

    public IEnumerator ResetCrosshairSize()
    {
        if (reticuleIsReleasing == true || reticuleIsSqueezing == true)
        {
            yield break;
        }
        reticuleIsReleasing = true;
        while (transform.localScale.x < defaultCursorSize.x)
        {
            float desiredCrosshairSize = Mathf.MoveTowards(transform.localScale.x, defaultCursorSize.x, Time.deltaTime * cursorAnimationSpeed);
            transform.localScale = new Vector3(desiredCrosshairSize, desiredCrosshairSize, desiredCrosshairSize);
            yield return new WaitForEndOfFrame();

        }
        reticuleIsReleasing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            ChangeCursorColor(enemyMouseOverColor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ChangeCursorColor(defaultCursorColor);
        }
    }

    private void ChangeCursorColor(Color targetColor)
    {
        for (int i = 0; i < spriteRenderersArray.Length; i++)
        {
            spriteRenderersArray[i].color = targetColor;
        }
    }


}

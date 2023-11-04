using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Currency,
        Powerup,
        Task
    }

    public PickupType type;

    [Header("VFX Prefabs")]
    public GameObject spawnVFX;
    public GameObject collectedVFX;
    public GameObject fizzleVFX;

    [Header("Stats")]
    public float pickupLifetime;


    protected SpriteRenderer[] spriteRenderers;

    protected virtual void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        FadeOut();
    }
    protected virtual void Start()
    {
        Invoke(nameof(Fizzle), pickupLifetime);
        GameManager.gameManagerInstance.pickupList.Add(this);

        SpawnVFX(spawnVFX);
    }



    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        Collect();


    }

    protected virtual void Collect()
    {
        GameManager.gameManagerInstance.pickupList.Remove(this);
        SpawnVFX(collectedVFX, true);
    }

    protected virtual void Fizzle()
    {
        GameManager.gameManagerInstance.pickupList.Remove(this);
        SpawnVFX(fizzleVFX, true);
    }

    protected void SpawnVFX(GameObject vfx, bool destroySelf = false)
    {
        GameObject activeVFX = Instantiate(vfx, transform.position, transform.rotation);
        Destroy(activeVFX, 2f);

        if(destroySelf == true)
        {
            Destroy(gameObject);
        }
        //lowercase gameObject always refers to the game object the script is attached to D:<
    }

    public void FadeOut()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            SpriteRenderer targetSpriteRenderer = spriteRenderers[i];
            StartCoroutine(SpriteUtilities.FadeSprite(0, targetSpriteRenderer, pickupLifetime));
        }
    }


}

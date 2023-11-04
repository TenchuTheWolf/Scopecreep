using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [Header("Point Value")]
    public float pointValue;
    public int minimumLootDrop = 0;
    public int maximumLootDrop = 1;


    [Header("Visual Effects")]
    public GameObject deathVFX;
    public GameObject spawnVFX;

    [Header("Drops")]
    public GameObject lootPrefab;

    [Header("Health & Healthbars")]
    protected Health health;
    public bool hasHealthBar = false;
    public Vector2 enemyhealthbarOffset;
    public float enemyhealthbarScale;
    public EnemyHealthbar enemyHealthbarPrefab;
    public EnemyHealthbar activeHealthBar;

    protected Rigidbody2D rigidBody;
    public bool untrackedNPC;
    public bool isAttractModeEnemy;

    [HideInInspector]
    public SpriteRenderer npcSpriteRenderer;

    public enum EnemyType
    {
        SmallAsteroid,
        MediumAsteroid,
        LargeAsteroid,
        Satellite,
        TechDebtMeteor,
        MiniDebtMeteor,
        Micromanager
    }

    public EnemyType enemyType;

    protected virtual void Start()
    {
        //This is called in a child class. Do not delete.
    }

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        npcSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (untrackedNPC == false)
        {
            GameManager.EnlistEnemy(this);
        }

        health = GetComponent<Health>();
        if(health == null)
        {
            Debug.LogError("The NPC: " +gameObject.name + " is missing a Health Script.");
        }

        if(hasHealthBar == true && isAttractModeEnemy == false)
        {
            GenerateNewHealthbar();
        }

        if (rigidBody == null)
        {
            Debug.LogError("A rigidbody script was missing on this asteroid.");
        }
    }

    protected virtual void Die()
    {
        if(activeHealthBar != null)
        {
            Destroy(activeHealthBar.gameObject);
        }

        switch (enemyType)
        {
            case EnemyType.SmallAsteroid:
                AudioManager.PlaySound("Small Bug Chip Death SFX", .5f);
                break;
            case EnemyType.MediumAsteroid:
                AudioManager.PlaySound("Med Bug Chip Death SFX", .5f);
                break;
            case EnemyType.LargeAsteroid:
                AudioManager.PlaySound("Large Bug Chip Death SFX", .5f);
                break;
            case EnemyType.Satellite:
                Debug.LogError("A satellite died but there was no audio SFX cue for it.");
                break;
            case EnemyType.TechDebtMeteor:
                AudioManager.PlaySound("Tech Debt Meteor Death SFX", .5f);
                break;
            case EnemyType.MiniDebtMeteor:
                Debug.LogError("A mini debt meteor died but there was no audio SFX cue for it. wtf is a mini debt meteor");
                break;
            case EnemyType.Micromanager:
                AudioManager.PlaySound("Micromanager Death SFX", .5f);
                break;
        }

        GameObject activeDeathVFX = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(activeDeathVFX, 2f);

        Player.playerInstance.UpdateScore(pointValue);

        //Enemies shouldn't drop loot during Dev Hell Mode to reduce resource use, and they won't get to use the design docs anyway.
        if(DevHellMode.devHellModeActive == false)
        {
            DropLoot();
        }

        if (untrackedNPC == false)
        {
            GameManager.UnlistEnemy(this);
        }

        LootManager.AddToThreshold(pointValue);

        Destroy(gameObject);
    }

    private void DropLoot()
    {
        //Random.Range excludes the maximum value on purpose, but only for integers. This is not well explained by its description.
        int lootDropTotal = Random.Range(minimumLootDrop, maximumLootDrop + 1);
        for (int i = 0; i < lootDropTotal; i++)
        {
            Instantiate(lootPrefab, transform.position, transform.rotation);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player Bullet")
        {
            health.ReduceCurrentHealth(1f, false);
        }
    }

    public virtual IEnumerator DieOnDelay()
    {
        yield return new WaitForEndOfFrame();
        Die();
    }

    public virtual void CleanupHealthBar()
    {
        if(activeHealthBar == true)
        {
            Destroy(activeHealthBar.gameObject);
        }
    }

    public void GenerateNewHealthbar()
    {
        activeHealthBar = Instantiate(enemyHealthbarPrefab, GameManager.gameManagerInstance.toolTipCanvas.transform);
        activeHealthBar.SetupHealthbar(this, this.transform);
    }


}

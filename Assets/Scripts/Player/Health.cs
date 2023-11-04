using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float minHealth = 0f;
    public float maxHealth;
    private float baseMaxHealth;
    public float currentHealth;
    private Player player;
    private NPC npc;
    public float HealthRatio { get; private set; }
    public bool isDead = false;

    private void Awake()
    {
        baseMaxHealth = maxHealth;
        currentHealth = maxHealth;
        player = GetComponent<Player>();
        npc = GetComponent<NPC>();
    }

    /// <summary>
    /// This method decreases the units current health using the provided amount.
    /// </summary>
    public void ReduceCurrentHealth(float amountToSubtract, bool hasHealthBar)
    {
        if (isDead == true)
        {
            return;
        }

        currentHealth -= amountToSubtract;

        if (player != null)
        {
            player.PlayDamageTakenEffects();
        }

        if (hasHealthBar == true)
        {
            CalculateHealthRatio();
        }

        if (npc != null && npc.hasHealthBar == true)
        {
            CalculateHealthRatio();
            npc.activeHealthBar.UpdateHealthbar(HealthRatio);
        }

        if (CheckIfDead() == true)
        {
            if (npc != null)
            {
                StartCoroutine(npc.DieOnDelay());
            }
        }
    }

    /// <summary>
    /// This increases the maximum health of the unit, by the provided value. 
    /// </summary>
    public void IncreaseMaxHealth(float amountToIncrease)
    {
        maxHealth += amountToIncrease;
        CalculateHealthRatio();
    }


    /// <summary>
    /// This method increases the units current health using the provided amount.
    /// </summary>
    public void IncreaseCurrentHealth(float amountToAdd, bool hasHealthBar)
    {
        currentHealth += amountToAdd;

        if (hasHealthBar == true)
        {
            CalculateHealthRatio();
        }
    }

    /// <summary>
    /// Resets the maximum health of the unit to the pre-set max value in their settings. This is for the player on restarts.
    /// </summary>
    public void ResetMaxHealth()
    {
        maxHealth = baseMaxHealth;
        CalculateHealthRatio();
    }



    /// <summary>
    /// You run this method after modifying the health of something to see if its dead or not. It returns a bool.
    /// </summary>
    public bool CheckIfDead()
    {
        //this should let me control when something dies from within the unit classes but still manage dead/alive states here.
        if (currentHealth <= minHealth)
        {
            isDead = true;
        }
        return isDead;
    }

    /// <summary>
    /// This method calculates the health ratio automatically and updates any relevant healthbars.
    /// </summary>
    public void CalculateHealthRatio()
    {
        HealthRatio = (1 / maxHealth) * currentHealth;

        if (gameObject.tag == "Player")
        {
            PanelManager.GetPanel<HUD>().UpdatePlayerUIHealthBar(HealthRatio);
        }
        if (gameObject.tag == "Enemy")
        {
            //some kind of panel manager call that gets the healthbar canvas on THIS enemy?
            //Do I stil need this for something?
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    //PlayerUpgradePanel will have buttons that interact with this manager to modify the player.
    private HUD playerHUD;
    public static UpgradeManager instance;

    //upgrade panel can check to see how many times these have been upgraded and if they are = 3 then it can remove or grey out that upgrade option.
    private int currentWepProjectileUpgradeCount = 0;
    private int currentWepCooldownUpgradeCount = 0;
    private int specWepBacklogRangeUpgradeCount = 0;
    private int specWepBacklogCooldownUpgradeCount = 0;
    private int shieldDurationUpgradeCount = 0;
    private int playerHealthUpgradeCount = 0;
    private int playerBoostUpgradeCount = 0;

    //replace all of the set values with these in the enable / reset
    [Header("Basic Weapon Default Settings")]
    public float defBasicWeaponProjCount = 1f;
    public float defBasicWeaponProjLifetime = 1f;
    public float defBasicWepProjCooldown = 1f;

    [Header("Basic Wep Shotgun Default Settings")]
    public float defShotgunWepProjCount = 3f;
    public float defShotgunWepProjLifetime = .5f;
    public float defShotgunWepProjCooldown = .3f;

    [Header("Basic Wep Shotgun Upgrade Increment Settings")]
    public float shotgunWepProjCountIncrement = 2f;
    public float shotgunWepProjCooldownIncrement = .05f;

    [Header("Basic Wep Novafire Default Settings")]
    public float defNovaWepProjCount = 5f;
    public float defNovaWepProjLifetime = .3f;
    public float defNovaWepProjCooldown = 1f;

    [Header("Basic Wep Nova Upgrade Increment Settings")]
    public float novaWepProjCountIncrement = 1f;
    public float novaWepProjCooldownIncrement = .15f;

    [Header("Basic Wep Volley Default Settings")]
    public float defVolleyWepProjCount = 2f;
    public float defVolleyWepProjLifetime = .7f;
    public float defVolleyWepProjCooldown = .5f;

    [Header("Basic Wep Volley Upgrade Increment Settings")]
    public float volleyWepProjCountIncrement = 1f;
    public float volleyWepProjCooldownIncrement = .1f;

    [Header("Backlog Missile Default Settings")]
    public float defBacklogMissileCooldown = 10f;
    public float defBacklogMissilePullRange = 35f;
    public float defBacklogMissileDamageRange = 20f;

    [Header("Backlog Missile Upgrade Increment Settings")]
    public float backlogMissileDamageRangeIncrement = 5f;
    public float backlogMissilePullRangeIncrement = 5f;
    public float backlogMissileCooldownIncrement = 1f;

    [Header("Shield Pickup Default Settings")]
    public float defShieldDuration = 1f;

    [Header("Shield Upgrade Increment Settings")]
    public float shieldDurationIncrement = 1f;

    [Header("Ship Boost Upgrade Default Settings")]
    public float defBoostCooldown = 7f;

    [Header("Ship Boost Upgrade Increment Settings")]
    public float boostCooldownIncrement = 1f;

    public List<UpgradeData> allUpgradeDataList = new List<UpgradeData>();
    public List<UpgradeData> startingUpgradePool = new List<UpgradeData>();
    public List<UpgradeData> CurrentUpgradeDataPool { get; private set; } = new List<UpgradeData>();
    public List<UpgradeData> purchasedUpgrades = new List<UpgradeData>();


    private void Awake()
    {
        //is this the right hud component to grab?
        //I shouldn't need to reset this since the playerHUD doesn't go away when the player dies?
        //if (playerHUD == null)
        //{
        //    playerHUD = GetComponent<HUD>();
        //}
        if(instance == null)
        {
            instance = this;
        }
        
        CurrentUpgradeDataPool.AddRange(startingUpgradePool);
    }

    public static UpgradeData GetUpgradeDataByID(UpgradeIDNumber ID)
    {
        for (int i = 0; i < instance.allUpgradeDataList.Count; i++)
        {
            if (ID == instance.allUpgradeDataList[i].upgradeID)
            {
                return instance.allUpgradeDataList[i];
            }
        }
        return null;
    }

    public static void AddUpgradeToPool(UpgradeIDNumber targetID)
    {
        UpgradeData dataToGet = GetUpgradeDataByID(targetID);
        instance.CurrentUpgradeDataPool.Add(dataToGet);
    }

    public static void RemoveUpgradeFromPool(UpgradeIDNumber targetID)
    {
        UpgradeData dataToRemove = GetUpgradeDataByID(targetID);
        if(instance.CurrentUpgradeDataPool.Contains(dataToRemove) == true)
        {
            instance.CurrentUpgradeDataPool.Remove(dataToRemove);
            return;
        }
        Debug.LogError("Tried to remove: " + dataToRemove.upgradeName + " from the currentUpgradeDataPool even though it is not there.");
    }

    public static void OnUpgradeSelected(UpgradeData upgradeToApply)
    {
        instance.purchasedUpgrades.Add(upgradeToApply);

        switch (upgradeToApply.upgradeID)
        {
            case UpgradeIDNumber.ShotgunUnlockLvl0:
                instance.EnableBasicWepShotgun();
                break;
            case UpgradeIDNumber.NovaShotUnlockLvl0:
                instance.EnableBasicWepNovaFire();
                break;
            case UpgradeIDNumber.VolleyShotUnlockLvl0:
                instance.EnableBasicWepVolleyFire();
                break;
            case UpgradeIDNumber.BacklogRocketUnlockLvl0:
                instance.EnableSpecWepBacklogMissile();
                break;
            case UpgradeIDNumber.ShotgunCountLvl1:
                instance.BuffWepShotgunProjCount();
                break;
            case UpgradeIDNumber.ShotgunCountLvl2:
                instance.BuffWepShotgunProjCount();
                break;
            case UpgradeIDNumber.ShotgunCountLvl3:
                instance.BuffWepShotgunProjCount();
                break;
            case UpgradeIDNumber.ShotgunCooldownLvl1:
                instance.BuffWepShotgunCooldown();
                break;
            case UpgradeIDNumber.ShotgunCooldownLvl2:
                instance.BuffWepShotgunCooldown();
                break;
            case UpgradeIDNumber.ShotgunCooldownLvl3:
                instance.BuffWepShotgunCooldown();
                break;
            case UpgradeIDNumber.NovaShotCountLvl1:
                instance.BuffWepNovaFireProjectiles();
                break;
            case UpgradeIDNumber.NovaShotCountLvl2:
                instance.BuffWepNovaFireProjectiles();
                break;
            case UpgradeIDNumber.NovaShotCountLvl3:
                instance.BuffWepNovaFireProjectiles();
                break;
            case UpgradeIDNumber.NovaShotCooldownLvl1:
                instance.BuffWepNovaFireCooldown();
                break;
            case UpgradeIDNumber.NovaShotCooldownLvl2:
                instance.BuffWepNovaFireCooldown();
                break;
            case UpgradeIDNumber.NovaShotCooldownLvl3:
                instance.BuffWepNovaFireCooldown();
                break;
            case UpgradeIDNumber.VolleyShotCountLvl1:
                instance.BuffWepVolleyFireProjectiles();
                break;
            case UpgradeIDNumber.VolleyShotCountLvl2:
                instance.BuffWepVolleyFireProjectiles();
                break;
            case UpgradeIDNumber.VolleyShotCountLvl3:
                instance.BuffWepVolleyFireProjectiles();
                break;
            case UpgradeIDNumber.VolleyShotCooldownLvl1:
                instance.BuffWepVolleyFireCooldown();
                break;
            case UpgradeIDNumber.VolleyShotCooldownLvl2:
                instance.BuffWepVolleyFireCooldown();
                break;
            case UpgradeIDNumber.VolleyShotCooldownLvl3:
                instance.BuffWepVolleyFireCooldown();
                break;
            case UpgradeIDNumber.ShieldGraceDurationLvl1:
                instance.BuffShieldPickup();
                break;
            case UpgradeIDNumber.ShieldGraceDurationLvl2:
                instance.BuffShieldPickup();
                break;
            case UpgradeIDNumber.ShieldGraceDurationLvl3:
                instance.BuffShieldPickup();
                break;
            case UpgradeIDNumber.BacklogRocketCooldownLvl1:
                instance.BuffWepBacklogMissileCooldown();
                break;
            case UpgradeIDNumber.BacklogRocketCooldownLvl2:
                instance.BuffWepBacklogMissileCooldown();
                break;
            case UpgradeIDNumber.BacklogRocketCooldownLvl3:
                instance.BuffWepBacklogMissileCooldown();
                break;
            case UpgradeIDNumber.BacklogRocketAreaLvl1:
                instance.BuffWepBacklogMissileRange();
                break;
            case UpgradeIDNumber.BacklogRocketAreaLvl2:
                instance.BuffWepBacklogMissileRange();
                break;
            case UpgradeIDNumber.BacklogRocketAreaLvl3:
                instance.BuffWepBacklogMissileRange();
                break;
            case UpgradeIDNumber.PlayerMaxHealthLvl1:
                instance.BuffPlayerMaxHealth();
                Player.playerInstance.PlayerHealth.CalculateHealthRatio();
                break;
            case UpgradeIDNumber.PlayerMaxHealthLvl2:
                instance.BuffPlayerMaxHealth();
                Player.playerInstance.PlayerHealth.CalculateHealthRatio();
                break;
            case UpgradeIDNumber.PlayerMaxHealthLvl3:
                instance.BuffPlayerMaxHealth();
                Player.playerInstance.PlayerHealth.CalculateHealthRatio();
                break;
            case UpgradeIDNumber.PlayerBoostCooldownLvl1:
                instance.BuffPlayerBoostCooldown();
                break;
            case UpgradeIDNumber.PlayerBoostCooldownLvl2:
                instance.BuffPlayerBoostCooldown();
                break;
            case UpgradeIDNumber.PlayerBoostCooldownLvl3:
                instance.BuffPlayerBoostCooldown();
                break;
            default:
                break;
        }
    }

    public void EnableBasicWepShotgun()
    {
        Player.playerInstance.currentWeaponType = WeaponType.Shotgun;
        Player.playerInstance.ChangeWeaponProjectile();
        Player.playerInstance.currentWepProjCount = defShotgunWepProjCount;
        Player.playerInstance.currentWepProjLifetime = defShotgunWepProjLifetime;
        Player.playerInstance.currentWepProjCooldown = defShotgunWepProjCooldown;

        CooldownUIManager.instance.CreateCooldownEntry(HUDIconType.Shotgun);
        //Change Basic Weapon Icon out
        //playerHUD.DisableBasicWepIcon();
        //playerHUD.EnableShotgunIcon();

    }

    public void BuffWepShotgunProjCount()
    {
        Player.playerInstance.currentWepProjCount += shotgunWepProjCountIncrement;
        currentWepProjectileUpgradeCount += 1;
    }

    public void BuffWepShotgunCooldown()
    {
        Player.playerInstance.currentWepProjCooldown -= shotgunWepProjCooldownIncrement;
        currentWepCooldownUpgradeCount += 1;
    }

    public void EnableBasicWepNovaFire()
    {
        Player.playerInstance.currentWeaponType = WeaponType.Nova;
        Player.playerInstance.ChangeWeaponProjectile();
        Player.playerInstance.currentWepProjCount = defNovaWepProjCount;
        Player.playerInstance.currentWepProjLifetime = defNovaWepProjLifetime;
        Player.playerInstance.currentWepProjCooldown = defNovaWepProjCooldown;

        CooldownUIManager.instance.CreateCooldownEntry(HUDIconType.Nova);
    }

    public void BuffWepNovaFireProjectiles()
    {
        Player.playerInstance.currentWepProjCount += novaWepProjCountIncrement;
        currentWepProjectileUpgradeCount += 1;
    }

    public void BuffWepNovaFireCooldown()
    {
        Player.playerInstance.currentWepProjCooldown -= novaWepProjCooldownIncrement;
        currentWepCooldownUpgradeCount += 1;
    }

    public void EnableBasicWepVolleyFire()
    {
        Player.playerInstance.currentWeaponType = WeaponType.Volley;
        Player.playerInstance.ChangeWeaponProjectile();
        Player.playerInstance.currentWepProjCount = defVolleyWepProjCount;
        Player.playerInstance.currentWepProjLifetime = defVolleyWepProjLifetime;
        Player.playerInstance.currentWepProjCooldown = defVolleyWepProjCooldown;

        CooldownUIManager.instance.CreateCooldownEntry(HUDIconType.Volley);
    }

    public void BuffWepVolleyFireProjectiles()
    {
        Player.playerInstance.currentWepProjCount += volleyWepProjCountIncrement;
        currentWepProjectileUpgradeCount += 1;
    }

    public void BuffWepVolleyFireCooldown()
    {
        Player.playerInstance.currentWepProjCooldown -= volleyWepProjCooldownIncrement;
        currentWepCooldownUpgradeCount += 1;
    }

    public void EnableSpecWepBacklogMissile()
    {
        Player.playerInstance.specWepBacklogMissEnabled = true;

        CooldownUIManager.instance.CreateCooldownEntry(HUDIconType.BacklogMissile);
    }
    public void BuffWepBacklogMissileRange()
    {
        Player.playerInstance.specWepBacklogMissDamageRange += backlogMissileDamageRangeIncrement;
        Player.playerInstance.specWepBacklogMissPullRange += backlogMissilePullRangeIncrement;
        specWepBacklogRangeUpgradeCount += 1;
    }

    public void BuffWepBacklogMissileCooldown()
    {
        Player.playerInstance.specWepBacklogMissCooldown -= backlogMissileCooldownIncrement;
        specWepBacklogCooldownUpgradeCount += 1;
    }

    public void BuffShieldPickup()
    {
        Player.playerInstance.shieldDuration += shieldDurationIncrement;
        shieldDurationUpgradeCount += 1;
    }

    public void BuffPlayerMaxHealth()
    {
        Player.playerInstance.PlayerHealth.maxHealth += 1;
        Player.playerInstance.PlayerHealth.currentHealth = Player.playerInstance.PlayerHealth.maxHealth;
        playerHealthUpgradeCount += 1;
    }

    public void BuffPlayerBoostCooldown()
    {
        Player.playerInstance.boostCooldown -= boostCooldownIncrement;
        playerBoostUpgradeCount += 1;
    }

    /// <summary>
    /// Called when the game is reset to restart accruing abiliies for the next playthrough.
    /// </summary>
    public void ResetAllUpgrades()
    {
        //Deactivate all basic weapon functionalities and benefits.
        Player.playerInstance.currentWeaponType = WeaponType.Basic;
        Player.playerInstance.ChangeWeaponProjectile();
        Player.playerInstance.punchThrough = false;

        Player.playerInstance.currentWepProjLifetime = defBasicWeaponProjLifetime;
        Player.playerInstance.currentWepProjCount = defBasicWeaponProjCount;
        Player.playerInstance.currentWepProjCooldown = defBasicWepProjCooldown;

        //Deactivate and reset all Backlog Missile benefits.
        Player.playerInstance.specWepBacklogMissEnabled = false;
        Player.playerInstance.specWepBacklogMissCooldown = defBacklogMissileCooldown;
        Player.playerInstance.specWepBacklogMissDamageRange = defBacklogMissileDamageRange;
        Player.playerInstance.specWepBacklogMissPullRange = defBacklogMissilePullRange;

        //Reset the base duration of the Shield Pickup
        Player.playerInstance.shieldDuration = defShieldDuration;

        //Reset the base duration of the Boost Cooldown
        Player.playerInstance.boostCooldown = defBoostCooldown;

        //Reset the basic weapon icon
        //Remove the extra weapon cooldown displays from the weapon bar at the bottom.

        //Reset the upgrade counts for the weapons/other features.
        currentWepProjectileUpgradeCount = 0;
        currentWepCooldownUpgradeCount = 0;
        specWepBacklogRangeUpgradeCount = 0;
        specWepBacklogCooldownUpgradeCount = 0;
        shieldDurationUpgradeCount = 0;
        playerHealthUpgradeCount = 0;
        playerBoostUpgradeCount = 0;

        //Reset the upgrade pool for the player.
        CurrentUpgradeDataPool.Clear();
        CurrentUpgradeDataPool.AddRange(startingUpgradePool);
    }

}

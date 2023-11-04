using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeIDNumber
{
    ShotgunUnlockLvl0,
    NovaShotUnlockLvl0,
    VolleyShotUnlockLvl0,
    BacklogRocketUnlockLvl0,
    ShotgunCountLvl1,
    ShotgunCountLvl2,
    ShotgunCountLvl3,
    ShotgunCooldownLvl1,
    ShotgunCooldownLvl2,
    ShotgunCooldownLvl3,
    NovaShotCountLvl1,
    NovaShotCountLvl2,
    NovaShotCountLvl3,
    NovaShotCooldownLvl1,
    NovaShotCooldownLvl2,
    NovaShotCooldownLvl3,
    VolleyShotCountLvl1,
    VolleyShotCountLvl2,
    VolleyShotCountLvl3,
    VolleyShotCooldownLvl1,
    VolleyShotCooldownLvl2,
    VolleyShotCooldownLvl3,
    ShieldGraceDurationLvl1,
    ShieldGraceDurationLvl2,
    ShieldGraceDurationLvl3,
    BacklogRocketCooldownLvl1,
    BacklogRocketCooldownLvl2,
    BacklogRocketCooldownLvl3,
    BacklogRocketAreaLvl1,
    BacklogRocketAreaLvl2,
    BacklogRocketAreaLvl3,
    PlayerMaxHealthLvl1,
    PlayerMaxHealthLvl2,
    PlayerMaxHealthLvl3,
    PlayerBoostCooldownLvl1,
    PlayerBoostCooldownLvl2,
    PlayerBoostCooldownLvl3,
    None
}
[CreateAssetMenu(fileName = "Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Upgrade Details")]
    public string upgradeName;
    public string upgradeDescription;
    public string upgradeAdvancedDetails;
    public int upgradeCost;
    public Sprite upgradeIcon;
    public UpgradeIDNumber upgradeID;

    [Header("Upgrades To Add")]
    public List<UpgradeIDNumber> upgradesToAdd;

    [Header("Upgrades To Remove")]
    public List<UpgradeIDNumber> upgradesToRemove;
}

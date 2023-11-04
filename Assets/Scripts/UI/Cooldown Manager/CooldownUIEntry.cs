using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUIEntry : MonoBehaviour
{

    public Image cooldownFill;
    public Image weaponSprite;
    
    public HUDIconType hudIcon;

    public void Setup(HUDIconType hudIcon, Sprite hudSprite)
    {
        this.hudIcon = hudIcon;
        this.weaponSprite.sprite = hudSprite;
    }

    private void Update()
    {
        if(Player.playerInstance == null)
        {
            return;
        }
        UpdateCooldownFill();
    }

    private void UpdateCooldownFill()
    {
        
        if(hudIcon == HUDIconType.BacklogMissile)
        {
            cooldownFill.fillAmount = Player.playerInstance.GetBacklogWeaponCooldownRatio();
        }
        if(hudIcon == HUDIconType.Boost)
        {
            cooldownFill.fillAmount = Player.playerInstance.GetBoostCooldownRatio();
        }
        if(hudIcon != HUDIconType.BacklogMissile && hudIcon != HUDIconType.Boost)
        {
            cooldownFill.fillAmount = Player.playerInstance.GetCurrentWeaponCooldownRatio();
        }
    }
}

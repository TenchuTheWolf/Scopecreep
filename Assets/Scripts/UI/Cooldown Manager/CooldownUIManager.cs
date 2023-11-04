using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HUDIconType
{
    Basic,
    Shotgun,
    Nova,
    Volley,
    BacklogMissile,
    Boost
}

public class CooldownUIManager : MonoBehaviour
{

    public CooldownUIEntry weaponCooldownPrefab;
    public Transform horizontalLayout;
    public List<IconSpriteMap> iconSpriteMap = new List<IconSpriteMap>();

    public static CooldownUIManager instance;

    private List<CooldownUIEntry> activeWeaponEntries = new List<CooldownUIEntry>();

    private void Awake()
    {
        instance = this;
    }

    public void CreateCooldownEntry(HUDIconType hudIcon)
    {
        Sprite targetSprite = GetSpriteByWeaponType(hudIcon);
        CooldownUIEntry newEntry = Instantiate(weaponCooldownPrefab, horizontalLayout);
        activeWeaponEntries.Add(newEntry);
        newEntry.Setup(hudIcon, targetSprite);
    }

    public void ClearCooldownEntries()
    {
        for (int i = 0; i < activeWeaponEntries.Count; i++)
        {
            Destroy(activeWeaponEntries[i].gameObject);
        }

        activeWeaponEntries.Clear();

    }

    private Sprite GetSpriteByWeaponType(HUDIconType hudIcon)
    {
        for (int i = 0; i < iconSpriteMap.Count; i++)
        {
            if (iconSpriteMap[i].hudIcon == hudIcon)
            {
                return iconSpriteMap[i].weaponSprite;
            }
        }
        return null;
    }

    [System.Serializable]
    public class IconSpriteMap
    {
        public HUDIconType hudIcon;
        public Sprite weaponSprite;

    }
}

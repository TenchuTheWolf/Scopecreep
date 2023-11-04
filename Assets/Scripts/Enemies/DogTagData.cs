using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTagData
{
    public NPC.EnemyType enemyType;
    public Sprite enemySprite;

    public DogTagData(NPC.EnemyType enemyType, Sprite enemySprite)
    {
        this.enemyType = enemyType;
        this.enemySprite = enemySprite;
    }
}



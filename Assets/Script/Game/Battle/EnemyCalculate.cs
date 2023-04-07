using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCalculate
{
    public static SpriteData GetEnemyData()
    {
        int someid = 1;
        SpriteData enemy = new SpriteData();
        enemy.id = someid;
        enemy.takeDefaultCardsID = someid;
        enemy.hp_cur = enemy.hp_max = 100;
        return enemy;
    }
}

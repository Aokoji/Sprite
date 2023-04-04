using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCalculate
{
    public static SpriteData GetEnemyData()
    {
        SpriteData enemy = new SpriteData();
        enemy.hp_cur = enemy.hp_max = 100;
        return enemy;
    }
}

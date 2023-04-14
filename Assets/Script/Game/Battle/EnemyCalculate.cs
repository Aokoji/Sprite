using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCalculate
{
    public static SpriteData GetEnemyData()
    {
        int someid = 1;

        SpriteData enemy = new SpriteData();
        enemy.id = 1;
        enemy.takeDefaultCardsID = someid;
        enemy.hp_cur = enemy.hp_max = 25;
        enemy.cost_cur = enemy.cost_max = 3;
        enemy.refreshData();
        return enemy;
    }

    /// <summary>
    /// 旧的引用被删除，传出的参数为待处理的cardEntity
    /// </summary>
    public static List<CardEntity> calculateEnemyAction(List<CardEntity> list,SpriteData data)
    {
        List<CardEntity> result = new List<CardEntity>();
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i]._data.cost <= data.cost_cur)
            {
                result.Add(list[i]);
                data.cost_cur -= list[i]._data.cost;
                list.Remove(list[i]);
                i--;
            }
        }
        return result;
    }
}

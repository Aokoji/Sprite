using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SpriteData
{
    public int id;
    public string sname;

    public int level;
    public int hp_max;
    public int hp_cur;
    public int def_cur;
    public int dodge;
    public int block;   //格挡
    public int speed;

    public int extraLimit;

    public int cost_max;
    public int cost_cur;
    public int takeDefaultCardsID;   //持有默认卡组
    public int spritePower;

    public void refreshData()
    {
        hp_cur = hp_max;
        cost_cur = cost_max;
        def_cur = 0;
    }

    public SpriteData Copy()
    {
        var copy = new SpriteData();
        copy.id = id;
        copy.sname = sname;
        copy.level = level;
        copy.hp_max = hp_max;
        copy.hp_cur = hp_cur;
        copy.def_cur = def_cur;
        copy.dodge = dodge;
        copy.block = block;
        copy.speed = speed;
        copy.extraLimit = extraLimit;
        copy.cost_max = cost_max;
        copy.cost_cur = cost_cur;
        copy.takeDefaultCardsID = takeDefaultCardsID;
        copy.spritePower = spritePower;
        return copy;
    }
}

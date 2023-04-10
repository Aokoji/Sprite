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

    public int cost_max;
    public int cost_cur;
    public int takeDefaultCardsID;   //持有默认卡组
}

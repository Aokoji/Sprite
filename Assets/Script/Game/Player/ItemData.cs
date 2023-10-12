using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ItemData
{
    public int id;
    public int num;
    public int rare;
    public int specialtype;     //目前0普通物品 1魔法书
    public int limitnum;    //内置数量 区别于num类似使用次数剩余。
    public string iconname;
    public ItemData(int tid,int tnum)
    {
        id = tid;
        num = tnum;
    }
    public void seticonString(string icon="")
    {
        if (icon == "")
            iconname = Config_t_items.getOne(id).iconName;
        else
            iconname = icon;
    }
}

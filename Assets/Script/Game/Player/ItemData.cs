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
    public string iconname;
    public int extra;   //辅助参数  //魔法书为对应卡

    //magic参数
    public int limitnum;    //内置数量 区别于num类似使用次数剩余。
    public int limitMax;
    public bool limitRound;


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
    public void uselimit(int num)
    {
        if (limitnum <= 0) return;
        limitnum = Mathf.Max(0, limitnum - num);
    }
    public void initmagic(int num)
    {
        var dat = Config_t_Consumable.getOne(num);
        limitMax = limitnum = dat.takenum2;
        extra = dat.takenum;
        limitRound = dat.rewardType == 1;
    }
}

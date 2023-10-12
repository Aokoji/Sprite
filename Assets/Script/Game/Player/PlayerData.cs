using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public int id;
    public string sname;

    public List<ItemData> playerAllCards;        //拥有的制作卡

    public List<ItemData> items;    //物品
    public List<SpriteData> sprites;
    public int curSprite;

    public List<WorkData> works;
    public TravelData travel;
    public MillData mill;
    public MarkData mark;
    public ExplorData explor;
    public Dictionary<int, ItemData> magicBookDic;
    public List<int> learndPlan;    //学会的设计图

    public int specialMarkNum;  //白给商店钱
    public void initdata()
    {
        playerAllCards = new List<ItemData>();
        sprites = new List<SpriteData>();
        items = new List<ItemData>();
        works = new List<WorkData>();
        //理论上有默认角色
        SpriteData sp = new SpriteData();
        sp.Convert_Data(Config_t_ActorMessage.getOne(1));
        sprites.Add(sp);
        sp = new SpriteData();
        sp.Convert_Data(Config_t_ActorMessage.getOne(2));
        sprites.Add(sp);
        sp = new SpriteData();
        sp.Convert_Data(Config_t_ActorMessage.getOne(3));
        sprites.Add(sp);
        sp = new SpriteData();
        sp.Convert_Data(Config_t_ActorMessage.getOne(4));
        sprites.Add(sp);
        sp = new SpriteData();
        sp.Convert_Data(Config_t_ActorMessage.getOne(5));
        sprites.Add(sp);
        curSprite = sp.id;
        travel = new TravelData();
        mill = new MillData();
        mark = new MarkData();
        explor = new ExplorData();
        magicBookDic = new Dictionary<int, ItemData>();
        learndPlan = new List<int>();
        //for(int i=0;i<20;i++)
        //   playerCards.Add(14);
    }

}

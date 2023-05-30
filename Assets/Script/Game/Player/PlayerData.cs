using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public int id;
    public string sname;

    public List<int> playerCards;
    public List<ItemData> playerAllCards;        //拥有的制作卡

    public List<ItemData> items;    //物品
    public List<SpriteData> sprites;
    public int curSprite;

    public TravelData travel;
    public MillData mill;

    public void initdata()
    {
        playerAllCards = new List<ItemData>();
        playerCards = new List<int>();
        sprites = new List<SpriteData>();
        items = new List<ItemData>();
        //理论上有默认角色
        SpriteData sp = new SpriteData();
        sp.id = 1;
        sp.hp_max = sp.hp_cur = 25;
        sp.cost_cur = sp.cost_max = 3;
        sp.takeDefaultCardsID = 1;
        sp.spritePower = 8;
        sprites.Add(sp);
        curSprite = sp.id;
        //for(int i=0;i<20;i++)
         //   playerCards.Add(14);
    }

}

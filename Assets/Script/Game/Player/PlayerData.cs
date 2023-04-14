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
    public List<int> playerAllCards;

    public List<SpriteData> sprites;
    public int curSprite;

    public void initdata()
    {
        playerAllCards = new List<int>();
        playerCards = new List<int>();
        sprites = new List<SpriteData>();
        //理论上有默认角色
        SpriteData sp = new SpriteData();
        sp.id = 1;
        sp.hp_max = sp.hp_cur = 25;
        sp.cost_cur = sp.cost_max = 3;
        sprites.Add(sp);
        curSprite = sp.id;
    }

    public void battleTest()
    {
        for (int i = 1; i < 9; i++)
        {
            playerCards.Add(i);
            playerCards.Add(i);
        }
            
    }
}

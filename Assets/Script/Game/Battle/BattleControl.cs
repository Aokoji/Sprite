using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl :Object
{
    private Dictionary<int, CardData> playercard;
    private int playerHP=60;//+++暂定  没有玩家类
    public void newbattle()
    {

    }

    private void getPlayerCardData()
    {
        playercard = new Dictionary<int, CardData>(PlayerManager.Instance.currentCardDic);
        
    }
}

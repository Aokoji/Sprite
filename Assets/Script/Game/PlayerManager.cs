using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamedata;

public class PlayerManager : CSingel<PlayerManager>
{
    public Dictionary<int, CardData> currentCardDic = new Dictionary<int, CardData>();
    public Dictionary<int, CardData> currentMagicDic = new Dictionary<int, CardData>();
    public int currentSprite;   //+++精灵类

    //测试
    private string CARD_TEST_PATH = "Asset/config/testpalyerCard";
    public void loadTestCardData()
    {
        currentCardDic = LoadDataAdapter.loadDataCard(CARD_TEST_PATH);
        foreach(var item in currentCardDic)
            if (item.Value.limit == CardLimitType.fatigue)
            {
                currentMagicDic.Add(item.Value.id, item.Value);
                currentCardDic.Remove(item.Value.id);
            }
    }
}

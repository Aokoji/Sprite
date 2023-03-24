using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamedata;

public class PlayerManager : CSingel<PlayerManager>
{
    public Dictionary<int, PlayerData> spritesDic = new Dictionary<int, PlayerData>();

    public Dictionary<int, CardData> currentCardDic = new Dictionary<int, CardData>();
    public Dictionary<int, CardData> currentMagicDic = new Dictionary<int, CardData>();
    public PlayerData currentSprite;
    public void init()
    {
        injectPlayer();
    }

    public void injectPlayer()
    {//暂定为读asset文件

        loadTestCardData();
    }
    private void loadDefaultData(int id)
    {

    }
    
    //测试
    private string CARD_TEST_PATH = "Assets/config/testplayerCard.csv";
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

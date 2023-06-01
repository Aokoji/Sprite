using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : CSingel<PlayerManager>
{
    //PlayerAsset playerAsset;
    PlayerData playerdata //元数据
    //  ------------    解析数据
    public SpriteData cursprite;
    public Dictionary<int, SpriteData> spriteList{ get; private set; }
    public Dictionary<int,int> playerMakenDic { get; private set; }
    public Dictionary<int,int> playerItemDic { get; private set; }

    public bool loadsuccess;

    public void init()
    {
        loadsuccess = false;
        injectPlayer();
    }

    public void injectPlayer()
    {//暂定为读asset文件  改了 读json
        spriteList = new Dictionary<int, SpriteData>();
        playerMakenDic = new Dictionary<int, int>();
        playerItemDic = new Dictionary<int, int>();
        loadTestCardData();
        //初始化sprite         *******************************************     初始化字典数据     ***************************
        playerdata.sprites.ForEach(item => { if (item.id == playerdata.curSprite) cursprite = item; });
        playerdata.playerAllCards.ForEach((item) => { playerMakenDic.Add(item.id, item.num); });
        playerdata.items.ForEach(item => { playerItemDic.Add(item.id, item.num); });
        loadsuccess = true;
    }
    
    //private string CARD_TEST_PATH = "config/assetdata/playersave";
    public void loadTestCardData()
    {
        var data= AssetManager.loadJson<PlayerData>(S_SaverNames.pdata.ToString());
        if (data!=null)
        {
            //playerAsset = AssetManager.loadAsset<PlayerAsset>(CARD_TEST_PATH);
            playerdata = data;
            foreach (var i in data.sprites)
                spriteList.Add(i.id, i);
        }
        else
        {
            //playerAsset = ScriptableObject.CreateInstance<PlayerAsset>();
            playerdata = new PlayerData();
            playerdata.initdata();
            //playerAsset.playdata = playerdata;
            //AssetManager.saveAsset(playerAsset, CARD_TEST_PATH + ".asset");
            savePlayerData();
        }
    }
    public void savePlayerData()
    {
        playerdata.items.Clear();
        foreach(var i in playerItemDic)
            playerdata.items.Add(new ItemData(i.Key, i.Value));

        playerdata.playerAllCards.Clear();
        foreach (var i in playerMakenDic)
            playerdata.playerAllCards.Add(new ItemData(i.Key, i.Value));
        AssetManager.saveJson(S_SaverNames.pdata.ToString(), playerdata); 
    }

    public List<int> getPlayerCards()
    {
        return playerdata.playerCards;
    }
    public SpriteData getSpriteData(int id)
    {
        if (spriteList.ContainsKey(id))
            return spriteList[id];
        else
        {
            PubTool.Log("==获取精灵id错误==");
            return null;
        }
    }
    public void setPlayerCards(List<int> cards)
    {
        playerdata.playerCards= cards;
        savePlayerData();
    }
    //更改物品
    public void addItems(List<ItemData> data)
    {
        data.ForEach(item =>
        {
            if (playerItemDic.ContainsKey(item.id))
                playerItemDic[item.id] += item.num;
            else
                playerItemDic.Add(item.id, item.num);
        });
        savePlayerData();
    }
    public void addItemsNosave(int id,int count)
    {
        if (playerItemDic.ContainsKey(id))
            playerItemDic[id] += count;
        else
            playerItemDic.Add(id, count);
    }
    //慎用
    public TravelData getplayerTravel()
    {
        return playerdata.travel;
    }
    //仅mill界面用
    public MillData Milldata { get { return playerdata.mill; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayerManager : CSingel<PlayerManager>
{
    //PlayerAsset playerAsset;
    PlayerData playerdata; //元数据
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
        playerdata.mill.paddingLv();
        playerdata.sprites.ForEach(item => { if (item.id == playerdata.curSprite) cursprite = item; spriteList.Add(item.id, item); });
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
        if (playerdata.travel.quest.Count > 0)
            Debug.Log(playerdata.travel.quest[0].endTime);
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
            PubTool.LogError("==获取精灵id错误==");
            return null;
        }
    }
    public void setPlayerCards(List<int> cards)
    {
        playerdata.playerCards= cards;
        savePlayerData();
    }
    #region 旅行
    public void travel_sprite(int id,int spend)
    {
        spriteList[id].istraveling = true;
        spriteList[id].phy_cur -= spend;
        savePlayerData();
    }
    public void travel_shut(int id, int spend)
    {
        spriteList[id].istraveling = false;
        spriteList[id].phy_cur += spend;
        savePlayerData();
    }
    public void travel_back(int id)
    {
        spriteList[id].istraveling = false;
        savePlayerData();
    }
    #endregion
    //更改物品
    public void addItems(List<ItemData> data)
    {
        data.ForEach(item =>
        {
            addItemsNosave(item.id, item.num);
        });
        savePlayerData();
    }
    public void addItems(int id, int count)
    {
        addItemsNosave(id, count);
        savePlayerData();
    }
    public void addItemsNosave(int id,int count)
    {
        if (playerItemDic.ContainsKey(id))
        {
            playerItemDic[id] = playerItemDic[id] + count;
            if (playerItemDic[id] <= 0)
            {
                playerItemDic.Remove(id);
            }
        }
        else
        {
            if (count > 0)
                playerItemDic.Add(id, count);
            else
                PubTool.LogError("添加物品有误");
        }
    }
    public int getItem(int id)
    {
        if (playerItemDic.ContainsKey(id))
        {
            return playerItemDic[id];
        }
        return 0;
    }
    //慎用
    public TravelData getplayerTravel()
    {
        return playerdata.travel;
    }
    public List<WorkData> getplayerWork()
    {
        return playerdata.works;
    }
    #region mill
    //仅mill界面用
    public MillData Milldata { get { return playerdata.mill; } }
    public void addMillMater1(int addnum)
    {
        if(playerdata.mill.pdid1>0)
        {
            playerdata.mill.endtime1 = (DateTime.Parse(playerdata.mill.endtime1).AddSeconds(Config_t_crop.getOne(playerdata.mill.pdid1).produceCoef * addnum)).ToString();
            playerdata.mill.pdnum1 += addnum;
            savePlayerData();
        }
    }
    public void createMillMater1(int id,int addnum,DateTime nowatime)
    {
        playerdata.mill.pdid1 = id;
        playerdata.mill.endtime1= nowatime.AddSeconds(Config_t_crop.getOne(id).produceCoef * addnum).ToString();
        playerdata.mill.pdnum1 = addnum;
        savePlayerData();
    }
    public void addMillMater2(int addnum)
    {
        if (playerdata.mill.pdid2 > 0)
        {
            playerdata.mill.endtime2 = (DateTime.Parse(playerdata.mill.endtime2).AddSeconds(Config_t_crop.getOne(playerdata.mill.pdid2).produceCoef * addnum)).ToString();
            playerdata.mill.pdnum2 += addnum;
            savePlayerData();
        }
    }
    public void createMillMater2(int id, int addnum, DateTime nowatime)
    {
        playerdata.mill.pdid2 = id;
        playerdata.mill.endtime2 = nowatime.AddSeconds(Config_t_crop.getOne(id).produceCoef * addnum).ToString();
        playerdata.mill.pdnum2 = addnum;
        savePlayerData();
    }
    public void collectMill1(int num)
    {
        addItemsNosave(Config_t_crop.getOne(playerdata.mill.pdid1).finishID, num);
        playerdata.mill.pdnum1 -= num;
        if (playerdata.mill.pdnum1 == 0)
        {
            playerdata.mill.pdid1 = 0;
        }
        savePlayerData();
    }
    public void collectMill2(int num)
    {
        addItemsNosave(Config_t_crop.getOne(playerdata.mill.pdid2).finishID, num);
        playerdata.mill.pdnum2 -= num;
        if (playerdata.mill.pdnum2 == 0)
        {
            playerdata.mill.pdid2 = 0;
        }
        savePlayerData();
    }
    public void upgradeMill(Action callback)
    {
        playerdata.mill.extendLv += 1;
        playerdata.mill.paddingLv();
        savePlayerData();
        callback?.Invoke();
    }
    public void WorkStart(WorkData work)
    {
        spriteList[work.spid].isworking = true;
        spriteList[work.spid].phy_cur -= work.spendPhy;
        playerdata.works.Add(work);
        savePlayerData();
    }
    public void WorkShut(WorkData work)
    {
        spriteList[work.spid].isworking = false;
        spriteList[work.spid].phy_cur +=(int)(work.spendPhy*0.75f);
        playerdata.works.Remove(work);
        savePlayerData();
    }
    public void WorkFinish(WorkData work)
    {
        spriteList[work.spid].isworking = false;
        playerdata.works.Remove(work);
        addItems(work.reward);
    }
    #endregion
}

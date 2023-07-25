using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using customEvent;

public class PlayerManager : CSingel<PlayerManager>
{
    //PlayerAsset playerAsset;
    PlayerData playerdata; //元数据
    //  ------------    解析数据
    private SpriteData cursprite;
    public Dictionary<int, SpriteData> spriteList{ get; private set; }
    public Dictionary<int,int> playerMakenDic { get; private set; }
    public Dictionary<int,int> playerItemDic { get; private set; }

    public Dictionary<CardSelfType, int> spriteLevelCardDic;        //当前妖精卡片等级
    public bool loadsuccess;

    public void init()
    {
        loadsuccess = false;
        EventAction.Instance.AddEventGather<int>(eventType.battleFinish_I, onbattle);
        injectPlayer();
    }
    public void injectPlayer()
    {//暂定为读asset文件  改了 读json
        spriteList = new Dictionary<int, SpriteData>();
        playerMakenDic = new Dictionary<int, int>();
        playerItemDic = new Dictionary<int, int>();
        spriteLevelCardDic = new Dictionary<CardSelfType, int>();
        loadTestCardData();
        //初始化sprite         *******************************************     初始化字典数据     ***************************
        playerdata.mill.paddingLv();
        playerdata.sprites.ForEach(item => { spriteList.Add(item.id, item); });
        playerdata.playerAllCards.ForEach((item) => { playerMakenDic.Add(item.id, item.num); });
        playerdata.items.ForEach(item => { playerItemDic.Add(item.id, item.num); });
        changeSprite(playerdata.curSprite);
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
    public SpriteData getcursprite(){ return cursprite; }
    public void changeSprite(int id)
    {
        playerdata.curSprite = id;
        cursprite = spriteList[id];
        calculateCardLevel();
    }
    void calculateCardLevel()
    {
        var data = Config_t_TakeCardLevel.getOne(cursprite.id);
        spriteLevelCardDic[CardSelfType.normal] = data.normal;
        spriteLevelCardDic[CardSelfType.fire] = data.fire;
        spriteLevelCardDic[CardSelfType.water] = data.water;
        spriteLevelCardDic[CardSelfType.thunder] = data.thunder;
        spriteLevelCardDic[CardSelfType.forest] = data.forest;
        spriteLevelCardDic[CardSelfType.arcane] = data.arcane;
        spriteLevelCardDic[CardSelfType.goden] = data.goden;
    }
    public List<int> getPlayerCards()
    {
        return playerdata.playerCards;
    }
    public int getOneCardNum(int id)
    {
        if (playerMakenDic.ContainsKey(id))
            return playerMakenDic[id];
        else
            return 0;
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
    public void addSpecialMarkNum(int num)
    {
        playerdata.specialMarkNum += num;
        savePlayerData();
    }
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
    #region mark
    public MarkData GetMarkData() { return playerdata.mark; }
    public void refreshNewMark()
    {
        TimeSpan time = new TimeSpan(23, 59, 59);
        playerdata.mark.savetime = DateTime.Parse(time.ToString()).ToString();
        playerdata.mark.saleID.Clear();
        System.Random random = new System.Random();
        if (random.Next(10) < 3)
            playerdata.mark.saleID.Add(TableManager.Instance.markDic[1][random.Next(TableManager.Instance.markDic[1].Count)]);
        if(random.Next(10)<4)
            playerdata.mark.saleID.Add(TableManager.Instance.markDic[2][random.Next(TableManager.Instance.markDic[2].Count)]);
        if (random.Next(10) < 5)
            playerdata.mark.saleID.Add(TableManager.Instance.markDic[3][random.Next(TableManager.Instance.markDic[3].Count)]);
        playerdata.mark.saleID.Add(TableManager.Instance.markDic[4][random.Next(TableManager.Instance.markDic[4].Count)]);
        if(playerdata.mark.saleID.Count<=3)
            playerdata.mark.saleID.Add(TableManager.Instance.markDic[4][random.Next(TableManager.Instance.markDic[4].Count)]);
        int num = 6 - playerdata.mark.saleID.Count;
        for(int i = 0; i < num; i++)
        {
            playerdata.mark.saleID.Add(TableManager.Instance.markDic[5][random.Next(TableManager.Instance.markDic[5].Count)]);
        }
        playerdata.mark.saledcount = 0;
        savePlayerData();
    }
    public void onMarkSale(int count)
    {//卖物品
        playerdata.mark.saledcount += count;
    }
    public void onMarkBuy(t_Business mark)
    {//买！
        playerdata.mark.saleID.Remove(mark.id);
        addItems(mark.itemid, mark.salenum);
    }
    #endregion
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
    #region workshop
    public void makenItem(int planid,int num,Action callback)
    {
        var plan = Config_t_Plan.getOne(planid);
        string[] strid = plan.needid.Split('|');
        string[] strnum = plan.needcount.Split('|');
        List<ItemData> list = new List<ItemData>();
        for(int i = 0; i < strid.Length; i++)
        {
            int id = int.Parse(strid[i]);
            int count = int.Parse(strnum[i]);
            addItems(id, -count * num);
        }
        if (plan.finishType == 1)
        {
            if (playerMakenDic.ContainsKey(plan.finishID))
                playerMakenDic[plan.finishID] += num;
            else
                playerMakenDic[plan.finishID] = num;
            savePlayerData();
            PanelManager.Instance.showTips3("获得法术卡：‘" + Config_t_DataCard.getOne(plan.finishID).sname + "'×" + num);
        }
        else
        {
            addItems(plan.finishID, num);
            PanelManager.Instance.showTips4(new List<ItemData>() { new ItemData(plan.finishID, num) });
        }
        callback?.Invoke();
    }
    public List<int> getLearnList() { return playerdata.learndPlan; }
    public bool LearnPlan(int id)
    {
        if (playerdata.learndPlan.Contains(id))
            return false;
        else
        {
            playerdata.learndPlan.Add(id);
            return true;
        }
    }
    #endregion
    #region explor
    public ExplorData getExplorData() { return playerdata.explor; }
    public void refreshNewDayExplor()
    {
        TimeSpan time = new TimeSpan(23, 59, 59);
        playerdata.explor.savetime = DateTime.Parse(time.ToString()).ToString();

        //刷新offer
        playerdata.explor.offer.Clear();
        System.Random random = new System.Random();
        int rand = random.Next(40);
        if (rand < 1) rand = 0;
        else if (rand <= 8) rand = 1;
        else if (rand <= 22) rand = 2;
        else if (rand <= 32) rand = 3;
        else rand = 4;
        for(int i = 0; i < rand; i++)
            playerdata.explor.offer.Add(new OfferData(random.Next(1,Config_t_Offer._data.Count)));
        //随机一个天气    1-9
        //  2   5   321
        rand = random.Next(33);
        int fintyp = 1;
        if (rand < 25) fintyp = rand / 5 + 2;
        else if (rand < 27) fintyp = 1;
        else if (rand < 30) fintyp = 7;
        else if (rand < 32) fintyp = 8;
        else if (rand < 33) fintyp = 9;
        playerdata.explor.mapType = fintyp;
        //填充day box count奖励
        var mapconfig = Config_t_ExplorMap.getOne(fintyp);
        string[] room = mapconfig.mapGiftPool.Split('-');
        string boxes = room[random.Next(room.Length)];
        room = boxes.Split('|');
        foreach(var i in room)
        {
            playerdata.explor.daygift.Add(int.Parse(i));
        }
        //day boss奖励
        room = mapconfig.mapDayBox.Split('|');
        playerdata.explor.dayboss = int.Parse(room[random.Next(room.Length)]);
    }
    //更改当前精灵
    public void restCurSprite(int num)
    {
        cursprite.phy_cur += num;
        cursprite.phy_cur = Mathf.Min(cursprite.phy_cur, cursprite.phy_max);
        savePlayerData();
    }
    //当前精灵体力状态变化
    public void minusCurSprite(int num)
    {
        cursprite.phy_cur -= num;
        cursprite.phy_cur = Mathf.Max(cursprite.phy_cur, 0);
        savePlayerData();
    }
    //保存新的探索背包
    public void setExplorBag(List<int> bag,Action callback=null)
    {
        playerdata.explor.explorBag = bag;
        savePlayerData();
        callback?.Invoke();
    }
    #endregion
    //战斗响应
    void onbattle(int id)
    {
        bool changed=false;
        foreach(var item in playerdata.explor.offer)
        {
            if (item.id == id)
            {
                changed = true;
                item.finishCount++;
            }
        }
        if (changed)
            savePlayerData();
    }
    //用东西
    public bool useProp(int index,int spid)
    {
        var config = Config_t_Consumable.getOne(index);
        var sp = spriteList[spid];
        if (config.takeid == 0)
        {
            //health
            if (sp.hp_cur == sp.hp_max)
                return false;
            sp.hp_cur += config.takenum;
            sp.hp_cur = Mathf.Min(sp.hp_cur, sp.hp_max);
        }
        if (config.takeid == 1)
        {
            //phy
            if (sp.phy_cur == sp.phy_max)
                return false;
            sp.phy_cur += config.takenum;
            sp.phy_cur = Mathf.Min(sp.phy_cur, sp.phy_max);
        }
        addItems(index, -1);
        return true;
    }
}

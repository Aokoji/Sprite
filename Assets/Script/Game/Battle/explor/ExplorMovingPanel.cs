﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using customEvent;

public class ExplorMovingPanel : PanelBase
{
    public GameObject startPos;
    public GameObject LineClone;
    public GameObject pointClone;
    public DirectionHelp directhelp;

    public Image spicon;
    public Text curphutext;
    public Button bagextra;

    public Button runbtn;

    public GameObject extrabar;
    public Button extrabgClose;
    public UITool_ScrollView scroll;
    public GameObject clone;
    //public List
    bool isway; //小径
    List<GameObject> points;
    List<GameObject> lines;     //统一销毁
    SpriteData cursp;
    bool isrefreshScroll;   //避免同界面反复开
    public override void init()
    {
        base.init();
        isway = (bool)message[0];
        points = new List<GameObject>();
        lines = new List<GameObject>();
        PlayerManager.Instance.getExplorData().dayboss_short = 0;
        cursp = PlayerManager.Instance.getcursprite();
        spicon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), cursp.icon);
        initCalculate();
        scroll.initConfig(200, 50, clone);
        refreshData();
        refreshScroll();
        closeExtraBar();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        bagextra.onClick.AddListener(bagsitempage);
        runbtn.onClick.AddListener(clickRun);
        extrabgClose.onClick.AddListener(closeExtraBar);
        EventAction.Instance.AddEventGather<bool>(eventType.explorGoNextRank_B, rankFinished);
    }
    public override void unregisterEvent()
    {
        base.unregisterEvent();
        EventAction.Instance.RemoveAction<bool>(eventType.explorGoNextRank_B, rankFinished);
    }
    public override void reshow()
    {
        base.reshow();
        isrefreshScroll = true;
        refreshData();
    }

    #region quick bag
    void refreshScroll()
    {
        StartCoroutine(reloadScroll());
    }
    IEnumerator reloadScroll()
    {
        scroll.recycleAll();
        var list = PlayerManager.Instance.getExplorData();
        for (int i = 0; i < list.explorBag.Count; i++)
        {
            var script = scroll.addItemDefault().GetComponent<ExplorQuickBagBar>();
            script.onused = onusedBag;
            ItemData conf = PlayerManager.Instance.getMagicBook(list.explorBag[i]);
            if (conf == null)
                script.setData(list.explorBag[i]);
            else
                script.setData(conf, true);
        }
        scroll.reCalculateHeigh();
        isrefreshScroll = false;
        yield return null;
    }
    void onusedBag()
    {
        isrefreshScroll = true;

    }
    void closeExtraBar()
    {
        extrabgClose.gameObject.SetActive(false);
        extrabar.SetActive(false);
    }
    #endregion
    void refreshData()
    {
        curphutext.text = cursp.phy_cur + "/" + cursp.phy_max;
    }
    public void rankFinished(bool finish)
    {
        if (finish)
        {
            if (currank.stype == explorIcon.boss)
            {
                var data = PlayerManager.Instance.getExplorData();
                if (data.dayboss_short > 0)
                {
                    PanelManager.Instance.showTips5("探索完成，获得每日奖励", new List<ItemData>() { new ItemData(data.dayboss_short, 1) }, jumpMainPanel);
                }
                else
                {
                    if (currank.sbox != null && currank.sbox.id != ConfigConst.explorExitBoxMaxMoneyID)
                    {
                        PlayerManager.Instance.getExplorData().daygift.Remove(currank.sbox.id);
                        PlayerManager.Instance.addItems(currank.sbox.id, currank.sbox.num);
                        PanelManager.Instance.showTips5("探索完成，得到了战利品", new List<ItemData>() { currank.sbox }, jumpMainPanel);
                    }
                    else
                    {
                        PanelManager.Instance.showTips5("探索完成", "即将返回哨站", jumpMainPanel);
                    }
                }
            }
            else
            {
                showDirectBtn();
            }
        }
        else
        {
            //战败
            PanelManager.Instance.showTips5("战斗失败", "损失最大体力的50%，即将返回哨站。", jumpMainPanel);
        }
    }

    void jumpMainPanel()
    {
        PanelManager.Instance.JumpPanelScene(E_UIPrefab.MainPanel, () =>
        {
            EventAction.Instance.TriggerAction(eventType.jumpMainExplor);
        });
    }
    void bagsitempage()
    {
        if(isrefreshScroll)
            refreshScroll();
        AnimationTool.playAnimation(extrabar, "movingBagShow");
        extrabgClose.gameObject.SetActive(true);
    }
    void clickRun()
    {
        PanelManager.Instance.showTips2("确定要返回哨站吗？", jumpMainPanel);

    }
    #region calculate
    //棋盘        3x3 4x4 5x4 5x5 5x6 6x6 
    Vector2 curPos;
    int curPoint;
    t_ExplorMapHelp mapconfig;
    float maxgap = 600;  //容器边长 矩形
    int width;
    int height;
    public enum direct { left,lu,up,ru,right,rd,down,ld, none }
    public class rankReward
    {
        public explorIcon stype;
        public int enemyID;
        public ItemData sbox;
        //新增  特殊地图或精英怪强化
        public bool isboss;
        public bool isexploring;    //用于悬赏，无特殊用途
    }
    rankReward currank;
    List<int> exitPos = new List<int>();
    List<int> rewardPos = new List<int>();
    int bossPos;
    void initCalculate()
    {
        curPos = startPos.transform.localPosition;
        var data = PlayerManager.Instance.getExplorData();
        string[] str;
        if (isway)
        {
            str = Config_t_ExplorMap.getOne(data.mapType).mapHelp.Split('|');
            mapconfig = Config_t_ExplorMapHelp.getOne(int.Parse(str[Random.Range(0, str.Length)]));
        }
        else
            mapconfig = Config_t_ExplorMapHelp.getOne(ConfigConst.explorWayID);
        width = (int)maxgap / mapconfig.mapwidth;
        height = (int)maxgap / mapconfig.mapheight;
        currank = new rankReward();
        currank.isexploring = true;
        //初始化位置数据
        str = mapconfig.exitNum.Split('|');
        foreach (var i in str)
            exitPos.Add(int.Parse(i));
        str=mapconfig.rewardNumber.Split('|');
        foreach(var i in str)
            rewardPos.Add(int.Parse(i));
        str=mapconfig.bossPos.Split('|');
        bossPos = int.Parse(str[Random.Range(0, str.Length)]);
        //哨站和方向
        curPoint = mapconfig.startPos;
        createANewPos(direct.none);
    }
    void directHelpCallBack(int id)
    {
        if (cursp.phy_cur < 2)
        {
            //体力不足
            PanelManager.Instance.showTips3("妖精体力不足！");
            return;
        }
        PlayerManager.Instance.minusCurSprite(ConfigConst.explorMoveSpend);
        PanelManager.Instance.showTips3("妖精体力-" + ConfigConst.explorMoveSpend);
        refreshData();
        currank.stype = explorIcon.outpost;
        //选项回调
        switch (id)
        {
            case 0:curPoint -= 1;break;
            case 1:curPoint += mapconfig.mapwidth-1;break;
            case 2:curPoint += mapconfig.mapwidth; break;
            case 3:curPoint += mapconfig.mapwidth + 1; break;
            case 4:curPoint += 1;break;
            case 5:curPoint -= mapconfig.mapwidth - 1; break;
            case 6:curPoint -= mapconfig.mapwidth; break;
            case 7:curPoint -= mapconfig.mapwidth + 1; break;
        }
        directhelp.gameObject.SetActive(false);
        //计算奖励
        System.Random random = new System.Random();
        //出口检测
        if (exitPos.Contains(curPoint))
        {
            if (curPoint == bossPos)
            {
                //boss每日奖励
                currank.stype = explorIcon.boss;
                var ems = mapconfig.bossPool.Split('|');
                currank.enemyID = int.Parse(ems[Random.Range(0, ems.Length)]);

                var data = PlayerManager.Instance.getExplorData();
                if (data.daygift.Count > 0 && data.dayboss == 0)
                    currank.sbox = new ItemData(data.daygift[random.Next(data.daygift.Count)], 1);
                else
                    currank.sbox = null;
            }
            else if (rewardPos.Contains(curPoint))
            {
                //普通宝箱
                currank.stype = explorIcon.exitBox;
                var data = PlayerManager.Instance.getExplorData();
                if (data.daygift.Count > 0)
                    currank.sbox = new ItemData(data.daygift[random.Next(data.daygift.Count)], 1);
                else
                    currank.sbox = new ItemData(ConfigConst.explorExitBoxMaxMoneyID,1);
            }
            else
            {
                //普通出口
                currank.stype = explorIcon.exit;
            }
        }
        //非出口则开始随机下一步事件
        else
        {
            //普通随机
            string[] weight = mapconfig.remark.Split('|');
            int result = random.Next(100);
            int count = 0;
            for (int i = 0; i < weight.Length; i++)
            {
                count += int.Parse(weight[i]);
                if (result < count)
                {
                    currank.stype = (explorIcon)(i + 1);
                    break;
                }
            }
            string[] ems;
            int itemid;
            //判断
            switch (currank.stype)
            {
                case explorIcon.battle:
                    ems = mapconfig.enemyPool.Split('|');
                    if (Main.NextEnemy > 0)
                        currank.enemyID = Main.NextEnemy;
                    else
                        currank.enemyID = int.Parse(ems[Random.Range(0, ems.Length)]);
                    break;
                case explorIcon.gather:
                    ems = mapconfig.gatherPool.Split('|');
                    itemid = int.Parse(ems[random.Next(ems.Length)]);
                    currank.sbox = new ItemData(itemid, random.Next(1, 4));
                    break;
                case explorIcon.box:
                    ems = mapconfig.boxPool.Split('|');
                    itemid = int.Parse(ems[random.Next(ems.Length)]);
                    currank.sbox = new ItemData(itemid, random.Next(1, 5));
                    break;
            }
        }

        createANewPos((direct)id);
    }
    void createANewPos(direct dir)
    {
        var obj = Instantiate(pointClone);
        points.Add(obj);
        obj.transform.SetParent(pointClone.transform.parent);
        obj.transform.localScale = Vector3.one;
        Vector2 vec = new Vector2(0,0);
        if (dir == direct.left || dir == direct.ld || dir == direct.lu)
            vec.x = -width;
        if (dir == direct.rd || dir == direct.ru || dir == direct.right)
            vec.x = width;
        if (dir == direct.up || dir == direct.lu || dir == direct.ru)
            vec.y = height;
        if (dir == direct.down || dir == direct.ld || dir == direct.rd)
            vec.y = -height;
        curPos += vec;
        obj.transform.localPosition = curPos;
        if (dir == direct.none)
        {
            obj.GetComponentInChildren<Text>().text = "森林哨站";
            //obj.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), explorIcon.outpost.ToString());
            AnimationTool.playAnimation(obj, "bornPoint", false, showDirectBtn);
            directhelp.initData(mapconfig, directHelpCallBack);
            showDirectBtn();
        }
        else
        {
            var line = Instantiate(LineClone);
            lines.Add(line);
            line.transform.SetParent(LineClone.transform.parent);
            line.transform.localScale = Vector3.one;
            line.transform.localEulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.right, vec));
            line.GetComponent<RectTransform>().sizeDelta = new Vector2(vec.magnitude - 40, 8);
            line.transform.localPosition = curPos - vec +vec.normalized*20;
            line.transform.SetAsFirstSibling();
            AnimationTool.playAnimation(line, "bornLine", false, ()=> {
                AnimationTool.playAnimation(obj, "bornPoint", false, showBattleReady);
            });
        }
    }

    void showBattleReady()
    {
        //展示战斗进入界面
        if(currank.stype==explorIcon.battle|| currank.stype == explorIcon.boss)
            PanelManager.Instance.OpenPanel(E_UIPrefab.ExplorBattleMessPanel, new object[] { currank });
        else if (currank.stype == explorIcon.exitBox)
        {
            if (currank.sbox.id != ConfigConst.explorExitBoxMaxMoneyID)
                PlayerManager.Instance.getExplorData().daygift.Remove(currank.sbox.id);
            PlayerManager.Instance.addItems(currank.sbox.id, currank.sbox.num);
            PanelManager.Instance.showTips5("到达森林出口，寻找到了战利品", new List<ItemData>() { currank.sbox }, jumpMainPanel);
        }
        else if (currank.stype == explorIcon.exit)
        {
            PanelManager.Instance.showTips5("到达森林出口，即将返回哨站", "", jumpMainPanel);
        }
        else if (currank.stype == explorIcon.rest)
        {
            PlayerManager.Instance.restCurSprite(cursp.phy_max/5);
            PanelManager.Instance.showTips5("稍作休息", "在魔力浓郁的地方休息了一会，恢复了20%的体力", () =>
            {
                EventAction.Instance.TriggerAction(eventType.explorGoNextRank_B, true);
            });
        }
        else
            PanelManager.Instance.OpenPanel(E_UIPrefab.ExplorGatherMessPanel, new object[] { currank });
    }
    void showDirectBtn()
    {
        directhelp.transform.localPosition = curPos;
        directhelp.transform.SetAsLastSibling();
        directhelp.showCurPosDirect(curPoint);
    }
    #endregion
    public override void Dispose()
    {
        lines.ForEach(item => { Destroy(item); });
        points.ForEach(item => { Destroy(item); });
        base.Dispose();
    }
}

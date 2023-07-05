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

    List<GameObject> points;
    List<GameObject> lines;     //统一销毁
    public override void init()
    {
        base.init();
        points = new List<GameObject>();
        lines = new List<GameObject>();
        initCalculate();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        EventAction.Instance.AddEventGather<bool>(eventType.explorGoNextRank_B, rankFinished);
    }
    public void rankFinished(bool finish)
    {
        if (finish)
        {
            if (currank.stype == explorIcon.boss)
            {
                //+++奖励
            }
            else if (currank.stype == explorIcon.exitBox)
            {

            }
            else
            {
                directhelp.showCurPosDirect(curPoint);
            }
        }
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
    }
    rankReward currank;
    void initCalculate()
    {
        curPos = startPos.transform.localPosition;
        var data = PlayerManager.Instance.getExplorData();
        string[] str = Config_t_ExplorMap.getOne(data.mapType).mapHelp.Split('|') ;
        mapconfig = Config_t_ExplorMapHelp.getOne(int.Parse(str[Random.Range(0, str.Length)]));
        width = (int)maxgap / mapconfig.mapwidth;
        height = (int)maxgap / mapconfig.mapheight;
        currank = new rankReward();
        //哨站和方向
        curPoint = mapconfig.startPos;
        createANewPos(direct.none);
    }
    void directHelpCallBack(int id)
    {
        if (PlayerManager.Instance.cursprite.phy_cur < 2)
        {
            //体力不足
            PanelManager.Instance.showTips3("妖精体力不足！");
            return;
        }
        PanelManager.Instance.showTips3("妖精体力-" + ConfigConst.explorMoveSpend);

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
        string[] str = mapconfig.rewardNumber.Split('|');
        foreach(var i in str)
        {
            int pos = int.Parse(i);
            if (curPoint == pos)
            {
                //奖励
                if (curPoint == mapconfig.bossPos)
                {
                    //boss每日奖励
                    currank.stype = explorIcon.boss;
                }
                else
                {
                    //普通宝箱
                    currank.stype = explorIcon.exitBox;
                }
                break;
            }
        }
        if (currank.stype == explorIcon.outpost)
        {
            //普通随机
            string[] weight = mapconfig.remark.Split('|');
            System.Random random = new System.Random();
            int result = random.Next(100);
            int count = 0;
            for(int i=0;i<weight.Length;i++)
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
                    currank.enemyID = int.Parse(ems[Random.Range(0, str.Length)]);
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
        PanelManager.Instance.OpenPanel(E_UIPrefab.ExplorBattleMessPanel, new object[] { currank });
    }
    void showDirectBtn()
    {
        directhelp.transform.localPosition = curPos;
        directhelp.transform.SetAsLastSibling();
        directhelp.showCurPosDirect(curPoint);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            createANewPos(direct.ru);
        if (Input.GetKeyDown(KeyCode.W))
            createANewPos(direct.right);
    }
    #endregion
    public override void Dispose()
    {
        lines.ForEach(item => { Destroy(item); });
        points.ForEach(item => { Destroy(item); });
        base.Dispose();
    }
}

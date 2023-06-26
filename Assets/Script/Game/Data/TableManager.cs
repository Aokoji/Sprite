using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TableManager : CSingel<TableManager>
{
    TableConfig config;
    public bool loadsuccess { get { return config.loadsuccess; } }

    public void init()
    {
        config = new TableConfig();
        config.init();
    }

    //商店预备字典
    public Dictionary<int, List<int>> markDic { get; private set; }
    //卡牌预备字典
    public Dictionary<CardSelfType, List<int>> stallCardDic { get; private set; }
    //任务品级
    public Dictionary<int, List<int>> questRankDic { get; private set; }
    public void LoadMessageData()
    {
        initAllCardStallTypeData();
        initQuestRankCheck();
        initMarkDic();
    }
    //初始化卡牌分类
    private void initAllCardStallTypeData()
    {
        stallCardDic = new Dictionary<CardSelfType, List<int>>();
        foreach (var i in Config_t_DataCard._data)
        {
            if (i.Value.type1 != CardType1.take && i.Value.type1 != CardType1.untaken) continue;
            if (!stallCardDic.ContainsKey(i.Value.limit))
            {
                stallCardDic.Add(i.Value.limit, new List<int>());
            }
            stallCardDic[i.Value.limit].Add(i.Value.id);
        }
    }
    //初始化任务分类
    private void initQuestRankCheck()
    {
        questRankDic = new Dictionary<int, List<int>>();
        foreach(var i in Config_t_quest._data)
        {
            if (!questRankDic.ContainsKey(i.Value.ranklevel))
            {
                questRankDic.Add(i.Value.ranklevel, new List<int>());
            }
            questRankDic[i.Value.ranklevel].Add(i.Value.id);
        }
    }

    void initMarkDic()
    {
        markDic = new Dictionary<int, List<int>>();
        foreach(var i in Config_t_Business._data)
        {
            if (!markDic.ContainsKey(i.Value.postype))
            {
                markDic[i.Value.postype] = new List<int>();
            }
            markDic[i.Value.postype].Add(i.Value.id);
        }
    }
}

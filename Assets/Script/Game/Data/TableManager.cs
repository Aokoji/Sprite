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

    public List<int> stall_normal = new List<int>();
    public List<int> stall_fire = new List<int>();
    public List<int> stall_water = new List<int>();
    public List<int> stall_thunder = new List<int>();
    public List<int> stall_forest = new List<int>();
    public List<int> stall_arcane = new List<int>();
    public List<int> stall_arcane_special = new List<int>();

    public Dictionary<int, List<int>> questRankDic { get; private set; }
    public void LoadMessageData()
    {
        initAllCardStallTypeData();
        initQuestRankCheck();
    }
    //初始化卡牌分类
    private void initAllCardStallTypeData()
    {
        foreach(var i in Config_t_DataCard._data)
        {
            if (i.Value.type1 != CardType1.take && i.Value.type1 != CardType1.untaken) continue;
            if (i.Value.limit == CardSelfType.normal)
                stall_normal.Add(i.Key);
            if (i.Value.limit == CardSelfType.fire)
                stall_fire.Add(i.Key);
            if (i.Value.limit == CardSelfType.water)
                stall_water.Add(i.Key);
            if (i.Value.limit == CardSelfType.thunder)
                stall_thunder.Add(i.Key);
            if (i.Value.limit == CardSelfType.forest)
                stall_forest.Add(i.Key);
            if (i.Value.limit == CardSelfType.arcane)
                stall_arcane.Add(i.Key);
            if (i.Value.limit == CardSelfType.arcane_special)
                stall_arcane_special.Add(i.Key);
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

}

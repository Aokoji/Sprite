using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class TravelManager : CSingel<TravelManager>
{
    public TravelData _data { get; private set; }

    public void init()
    {
        //展开数据
        //_data = AssetManager.loadJson<TravelData>(S_SaverNames.entrust.ToString());
        _data = PlayerManager.Instance.getplayerTravel();
        if (_data == null)
        {
            _data = new TravelData();
            _data.initdata();
            //saveTravel();
        }
    }
    //public void saveTravel() { AssetManager.saveJson(S_SaverNames.entrust.ToString(), _data); }
    public void completeRemoveQuest(int id)
    {
        for(int i = 0; i < _data.quest.Count; i++)
        {
            if (id == _data.quest[i].questID)
            {
                _data.quest.Remove(_data.quest[i]);
                Debug.Log("removed");
                break;
            }
        }
    }
    public bool goTravel(int spid,int square)
    {
        //检测可出发
        if (_data.quest.Count >= ConfigConst.QUEST_MAX)
        {
            PanelManager.Instance.showTips3("委托板已满！");
            return false;
        }
        //时间记录
        RunSingel.Instance.getBeiJingTime((result) =>
        {
            //回调
            QuestData dat = new QuestData();
            dat.spID = spid;
            //任务奖励和品级
            calculateTravelSpend(square, dat);
            //时间
            int spend = Config_t_TravelRandom.getOne(square).spendTime;
            if (dat.extraID > 0) spend = (int)(spend * 1.1f);
            dat.spFinish = result.AddMinutes(Config_t_TravelRandom.getOne(square).spendTime);
            spend = Config_t_quest.getOne(dat.questID).aliveTime;
            dat.endTime = dat.spFinish.AddMinutes(spend);
            //计算摆放位置
            List<int> pos = new List<int>();
            foreach(var i in _data.quest)
                pos.Add(i.pagePos);
            for(int i = 0; i < ConfigConst.QUEST_MAX; i++)
            {
                if (!pos.Contains(i))
                {
                    dat.pagePos = i;
                    break;
                }
            }
            _data.quest.Add(dat);
            EventAction.Instance.TriggerAction(eventType.spriteTravelComplete);
        });
        return true;
    }

    void calculateTravelSpend(int square, QuestData dat)
    {
        //普通任务 主线需要单算
        var sq = Config_t_TravelRandom.getOne(square);
        System.Random random = new System.Random();
        //计算幸运值 触发稀有素材 数量再说
        int luck = PlayerManager.Instance.spriteList[dat.spID].lucky;
        #region 奖励计算
        //普通奖励计算
        bool reward1 = luck >= random.Next(30);
        if (reward1)
        {
            string[] st1= sq.items.Split('|');
            int re1 = int.Parse(st1[random.Next(st1.Length)]);
            dat.takeItem.Add(re1);
            if(luck >= random.Next(36))
            {
                re1 = int.Parse(st1[random.Next(st1.Length)]);
                dat.takeItem.Add(re1);
            }
        }
        //特殊奖励
        luck = luck * luck;
        //假设地区标准    800-1500浮动 标准1000
        if(luck>= random.Next(sq.randomGap))
        {
            //lucky！
            string[] str = sq.itemsRare.Split('|');
            int awardid = int.Parse(str[random.Next(str.Length)]);
            dat.extraID = awardid;
        }
        #endregion
        #region 任务计算
        //内定任务品级参数  ss,s,a,b,c,d,e,f  8-0
        luck = PlayerManager.Instance.spriteList[dat.spID].level;
        luck = luck * luck;
        int finalLevel = 7;
        if(luck< random.Next(ConfigConst.Quest_SS))
        {
            finalLevel--;
            if (luck < random.Next(ConfigConst.Quest_S))
            {
                finalLevel--;
                if (luck < random.Next(ConfigConst.Quest_A))
                {
                    finalLevel--;
                    if (luck < random.Next(ConfigConst.Quest_B))
                    {
                        finalLevel--;
                        if (luck < random.Next(ConfigConst.Quest_C))
                        {
                            finalLevel--;
                            if (luck < random.Next(ConfigConst.Quest_D))
                            {
                                finalLevel--;
                                if (luck < random.Next(ConfigConst.Quest_E))
                                {
                                    finalLevel--;
                                }
                            }
                        }
                    }
                }
            }
        }
        dat.questID = TableManager.Instance.questRankDic[finalLevel][random.Next(TableManager.Instance.questRankDic[finalLevel].Count)];
        #endregion
    }
}

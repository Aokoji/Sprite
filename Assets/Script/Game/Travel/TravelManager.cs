using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelManager : CSingel<TravelManager>
{
    TravelData _data;

    public void init()
    {
        _data = AssetManager.loadJson<TravelData>(S_SaverNames.entrust.ToString());
        if (_data == null)
        {
            _data = new TravelData();
            _data.initdata();
            saveTravel();
        }
        refreshTravel();
    }
    public void saveTravel() { AssetManager.saveJson(S_SaverNames.entrust.ToString(), _data); }
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
            TravelitemData dat = new TravelitemData();
            dat.spID = spid;
            calculateTravelSpend(square, dat);
            dat.finish = result.AddSeconds(Config_t_TravelRandom.getOne(square).spendTime);
        });
        return true;
    }

    void refreshTravel()
    {
        RunSingel.Instance.getBeiJingTime((result) =>
        {
            //回调

        });
    }

    void calculateTravelSpend(int square, TravelitemData dat)
    {
        //普通任务 主线需要单算
    }
}

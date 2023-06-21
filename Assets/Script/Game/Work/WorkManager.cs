using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : CSingel<WorkManager>
{
    public List<WorkData> _data { get; private set; }
    Dictionary<e_workSquare, WorkData> workinglist;

    public void init()
    {
        workinglist = new Dictionary<e_workSquare, WorkData>();
        _data = PlayerManager.Instance.getplayerWork();
        if (_data.Count > 0)
            _data.ForEach(item => { workinglist.Add((e_workSquare)item.workSquare, item); });
    }

    public WorkData getSquareWork(e_workSquare square)
    {
        WorkData result = null;
        if (workinglist.ContainsKey(square))
            result = workinglist[square];
        return result;
    }

    public void WorkStart(e_workSquare square, int id,Action callback)
    {
        WorkData work = new WorkData();
        //读配置
        //e_workSquare的index就是序号
        var config = Config_t_TravelRandom.getOne((int)square);
        RunSingel.Instance.getBeiJingTime(result =>
        {
            work.spid = id;
            work.spendPhy = config.spendPhy;
            work.workSquare = (int)square;
            work.endtime = result.AddMinutes(config.spendTime).ToString();
            work.reward = getworkReword(config);
            PlayerManager.Instance.WorkStart(work);
            workinglist.Add(square, work);
            callback?.Invoke();
        });
    }
    public void WorkShut(e_workSquare square)
    {
        if (workinglist.ContainsKey(square))
        {
            PlayerManager.Instance.WorkShut(workinglist[square]);
            workinglist.Remove(square);
        }
        else
        {
            PubTool.LogError("取消错误");
        }
    }
    public void WorkFinish(e_workSquare square)
    {
        if (workinglist.ContainsKey(square))
        {
            PlayerManager.Instance.WorkFinish(workinglist[square]);
            PanelManager.Instance.showTips4(workinglist[square].reward);
            workinglist.Remove(square);
        }
        else
        {
            PubTool.LogError("完成错误");
        }
    }

    List<ItemData> getworkReword(t_TravelRandom config)
    {
        List<ItemData> list = new List<ItemData>();
        System.Random r = new System.Random();
        int num = r.Next(3);
        num = config.randomGap + num - 2;   //最终数量
        string[] strid = config.items.Split('|');
        Dictionary<int, int> items = new Dictionary<int, int>();
        for(int i = 0; i < num; i++)
        {
            int index = r.Next(strid.Length);
            int id = int.Parse(strid[index]);
            if (items.ContainsKey(id))
                items[id]++;
            else
                items[id] = 1;
        }
        foreach(var i in items)
        {
            list.Add(new ItemData(i.Key, i.Value));
        }
        return list;
    }
}

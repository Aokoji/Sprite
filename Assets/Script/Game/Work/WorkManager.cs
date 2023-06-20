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

    public void WorkStart(e_workSquare square, int id)
    {
        WorkData work = new WorkData();
        //读配置
        //workinglist.Add()
    }
}

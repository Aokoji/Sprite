using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorkData : BaseData
{
    public int spid;
    public string endtime;
    public int workSquare;
    public int spendPhy;
    public List<ItemData> reward = new List<ItemData>();
}

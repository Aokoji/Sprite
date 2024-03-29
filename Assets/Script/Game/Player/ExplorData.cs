﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ExplorData : BaseData
{
    public int mapType; //今日地图
    public string savetime;
    public List<int> explorBag = new List<int>();

    public List<OfferData> offer=new List<OfferData>();
    public List<int> daygift = new List<int>(); //宝箱id
    public int dayboss; //通关奖励宝箱
    public int dayboss_short;
}

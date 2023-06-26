using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MarkData : BaseData
{
    public List<int> saleID=new List<int>();
    public int saledcount;  //已售
    public string savetime;
}

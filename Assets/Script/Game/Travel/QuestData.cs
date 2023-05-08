using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData : BaseData
{
    public int id;
    public int findingType;
    public int curFinishNum;    //特殊字段
    public DateTime endTime;
}

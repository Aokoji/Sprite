﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TravelData : BaseData
{
    public int worldQuestStep;  //世界任务进度
    public int worldQuestRate;  //世界任务积累进度（满了会触发）

    public List<QuestData> quest = new List<QuestData>();
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData : BaseData   //委托单数据
{
    public int questID;         //任务id
    public int spID;    //精灵id
    public int squareID;    //地区
    public DateTime spFinish;   //精灵完成时间
    public DateTime endTime;    //任务结束时间
    public int pagePos; //委托单位置

    public List<int> takeItem = new List<int>();    //额外物品
    public int extraID;
    public bool isGet;  //已领取？

    public int curFinishNum;    //特殊字段

    public bool checkSquare(int id) { return squareID == id; }
}

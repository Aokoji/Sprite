using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MillData : BaseData
{
    public int workingID1;
    public int workingID2;
    public bool isupgrade;
    //精灵工作时间为固定消耗，在磨坊工作消耗固定5小时，x魔力  能够加速当前生产的速度并且产生若干精制材料。
    public string endtime1;
    public string endtime2;
    public int pdid1;    //生产id
    public int pdnum1;   //生产总数
    public int pdid2;    //生产id
    public int pdnum2;   //生产总数

    public int extendLv;    //扩建等级

    public int willMillCount1;   //基础20
    public int finishMillCount1;//20
    public int willMillCount2;   //基础20
    public int finishMillCount2;//20
    public int savemana;    //减少消耗
    //1 增加填料储料槽大小   +10
    //2 增加填料储料槽大小   +20
    //3 增加工作位，填料位   基础25
    //4 增加填料10，10
    //5 20，10   减少工作魔力消耗15%

    public void paddingLv()
    {
        isupgrade = false;
        savemana = 0;
        //计算实际值
        switch (extendLv)
        {
            case 0:willMillCount1 = finishMillCount1 = 20;break;
            case 1:willMillCount1 = finishMillCount1 = 30;break;
            case 2:willMillCount1 = finishMillCount1 = 50;break;
            case 3:isupgrade = true; willMillCount1 = finishMillCount1 = 50; willMillCount2 = finishMillCount2 = 25; break;
            case 4:isupgrade = true; willMillCount1 = finishMillCount1 = 60; willMillCount2 = finishMillCount2 = 35; break;
            case 5:isupgrade = true; willMillCount1 = finishMillCount1 = 80; willMillCount2 = finishMillCount2 = 45;savemana = 15; break;
        }
    }
}

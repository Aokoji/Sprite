using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gamedata
{
    public class BaseData
    {
        public void padding()
        {
            
        }
    }

    public enum CardType1
    {
        take,       //直接生效
        condition,  //条件
        condition_two,  //2条件
        condition_special,  //多条件
    }
    public enum CardLimitType
    {
        own,    //无限制
        onceLoop,   //单次无损耗
        used,   //消耗的
        fatigue,//耐久的
    }
}


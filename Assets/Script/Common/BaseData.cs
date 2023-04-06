using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData
{
    protected string TABLE_PATH = "Assets/config/";
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
public enum CardType2
{
    n_hit,  //直伤0
    n_preempt,  //先制1
    n_continuous,   //连击2
    n_thump,    //重击3
    n_recover,  //恢复4
    n_defence,  //甲5
    n_counter,  //反制6
    n_deal, //抽牌7

    e_decounter, //反counter
}



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
public class AssetData:ScriptableObject
{
    public object parse(string context,string typename)
    {
        switch (typename)
        {
            case "int":return int.Parse(context);
            case "string":return context;
            case "float":return float.Parse(context);
            case "CardType1":return (CardType1)int.Parse(context);
            case "CardType2": return (CardType2)int.Parse(context);
            case "CardLimitType": return (CardLimitType)int.Parse(context);
            default:return null;
        }
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
    n_defence,  //护盾5
    n_counter,  //诅咒6
    n_deal, //抽牌7
    e_deplete,  //魔力枯竭  8
    e_gift, //获得卡 9
    e_addition, //填卡    10
    e_defend,   //屏障    11
    e_power,    //秘术    12
    e_decounter, //反counter  13真言
	//火焰 潮汐 雷霆 森林 奥术
}



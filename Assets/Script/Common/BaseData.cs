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
            case "CardSelfType": return (CardSelfType)int.Parse(context);
            default:return null;
        }
    }
}
public enum CardType1
{
    take,       //非集换
    condition,  //单次
    condition_two,  //培养
    condition_special,  //消耗魔法书
    condition_special1,  //普通魔法书不消耗
}
public enum CardSelfType
{
    normal,
    fire,
    water,
    thunder,
    forest,
    arcane,//奥术
    arcane_special,//奥术
}
public enum CardType2
{
    n_hit,  //直伤0
    n_preempt,  //先制1   空出来damage1
    n_continuous,   //连击2   空出来damage1
    n_thump,    //重击3
    n_recover,  //恢复4
    n_defence,  //护盾5
    n_counter,  //诅咒6
    n_deal, //抽牌7
    e_deplete,  //魔力枯竭  8
    e_gift, //获得卡 9
    e_addition, //填卡    10+++
    e_defend,   //屏障    11
    d_power,    //秘术    12
    d_decounter, //反counter  13真言
    s_double, //  14翻倍
    n_broke, //  15破甲
    e_giftone, //  16定向获得
    s_weapon    //17 装备+++
    //标识最优先     真言  其次--    先制，诅咒，秘术，获得卡
}



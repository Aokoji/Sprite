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
    take,       //非集换   //默认卡
    condition,  //单次    //制作卡
    hidden,     //特殊卡 不显示
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
    none,
    n_hit,  //直伤1
    n_preempt,  //先制2   空出来damage1
    n_continuous,   //连击3   空出来damage1
    n_thump,    //重击4
    n_recover,  //恢复5
    n_defence,  //护盾6
    n_counter,  //诅咒7
    n_deal, //抽牌8
    e_deplete,  //魔力枯竭  9
    e_gift, //获得卡 10
    e_addition, //填卡    11+++
    e_defend,   //屏障    12
    d_power,    //秘术    13
    d_decounter, //反counter  14真言
    s_double, //  15翻倍
    n_broke, //  16破甲
    e_giftone, //  17定向获得
    s_weapon,    //18 装备+++
    s_blessup,        //19祝福+++


    n_enemydeal,
    //标识最优先     真言  其次--    先制，诅咒，秘术，获得卡

    //直伤，先制，恢复，护盾，抽牌，填卡，反制，屏障，定向获得    二类可以直接配0
}



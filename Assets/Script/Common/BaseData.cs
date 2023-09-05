using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseData
{
    protected string TABLE_PATH = "config/";
    public void padding()
    {
            
    }
    public virtual void initdata() { }

    public DateTime getDate(string dat)
    {
        return DateTime.Parse(dat);
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
            case "bool":return context.Equals("0") ? false : true;
            case "CardType1":return (CardType1)int.Parse(context);
            case "CardType2": return (CardType2)int.Parse(context);
            case "CardSelfType": return (CardSelfType)int.Parse(context);
            case "ItemsType":return (ItemsType)int.Parse(context);
            case "ItemType2": return (ItemType2)int.Parse(context);
            default:return null;
        }
    }
}
public enum CardType1
{
    take,       //0非集换   //默认卡      显示属性  
    untaken,    //1制作
    condition,  //2限定卡    //制作卡 全职业卡 mana点数卡
    hidden,     //3特殊卡 不显示(不进卡池
    condition_hidden,  //4限定且不进抽卡池（很特殊
    condition_magic,  //5消耗魔法书
    condition_book,  //6普通魔法书不消耗
    monster,        //7敌人特有卡
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
    goden,      //神术
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
    s_etch,     //20腐蚀+++
    s_boost,	//21增强下一张
    s_todefen,	//22对护甲

    g_return,   //23神术 等额返还
    g_umbra,    //24暗影治疗  治疗=伤害     <100等额，100-200损失百分比，200-300上限百分比

    e_giftTwo,      //25定向2  两张
    e_giftThree,      //26定向3  三张
    g_overcrue,     //27过量治疗    <100定额，100-200上限百分比
    s_arcaneOn,       //28奥术强化    充能
    s_arcaneOff,       //29奥术强化    消耗掉

    n_enemydeal,
    //标识最优先     真言  其次--    先制，诅咒，秘术，获得卡

    //直伤，先制，恢复，诅咒，护盾，抽牌，填卡，屏障，定向获得    二类可以直接配0
    //连击，重击，枯竭，获得卡，秘术，反，翻倍，破甲，装备，祝福     需要配
}

public enum e_workSquare        //t_TravelRandom表合并了旅行和工作
{
    travel=0,
    workmill=6,
    workmill2=7,
    workfarm=8,
}
public enum ItemsType
{
    none,       //货币类
    material,   //材料    1
    consum,     //消耗品   2(直接消耗)
    card,   //卡片    3
    magic,      //魔法书   4
}
public enum ItemType2
{
    coin,       //货币    0
    maken,  //炼金材料  1
    produce,    //生产    2
    build,      //建材    3
    plan,   //设计图   4
    happy,      //精灵玩具  5
    stone,  //魔石类    6  (压缩类
    food,       //食物    7
    box,        //礼盒    8
    letter,     //卷轴    9
    use,        //使用    10
    explor,     //冒险用品  11
}
public enum explorType
{
    none,   //不行
    easy1,
    easy2,
    easy3,
    normal1,
    normal2,
    hard1,
    hard2,
    boss
}
//悬赏类型
public enum offerType
{
    battle, //跳转战斗
    count,  //计数
    change,     //物品
}
public enum explorIcon
{
    outpost,    //哨站
    battle, //战斗
    rest,
    gather,     //采集
    box,
    boss,   //普通boss
    elite,  //精英
    monster,    //巨兽
    lord,   //领主
    exitBox,
    exit,
}
public enum spriteChooseType
{
    changeCur,  //切换精灵
    useSth,     //使用物品
}
//任务类型  0



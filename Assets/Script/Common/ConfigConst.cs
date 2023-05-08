using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigConst
{
    //战斗与抽卡
    public static int maxCardHand = 6;    //     手牌最大
    public static int maxCardTake = 4;    //     take最大
    public static float cardwait = 1.25f;//    抽一张牌需要等待
    public static float cardtime_dealtoshow = 0.5f; //  抽卡到展示
    public static float cardtime_showstay = 0.7f;   //展示时常
    public static float cardtime_showtohand = 0.4f; //展示完毕到手上
    public static float cardtime_refshMove = 0.25f; //刷新卡速度
    public static float cardtime_effectShow = 1.25f; //卡生效放大展示时间
    public static float cardtime_effectMove = 0.4f; //攻击粒子飞行时间
    public static float cardtime_effectMoveSlow = 0.6f; //攻击粒子慢飞行时间
    public static float cardtime_addition = 0.8f; //填卡

    public static int dealcard_constID = 2;    //魔法糖（抽卡）默认id
    public static int dealcard_useUp = 3;    //疲劳抽卡 默认id
    public static int dealcard_blessed = 91;    //仙灵祝福 默认id
    public static int dealcard_blessGod = 92;    //神灵祝福 默认id

    public static int QUEST_MAX= 6;    //任务上限

    public static Vector3 colorForest = new Vector3(164,210,146);
    public static Vector3 colorFire = new Vector3(215,129,125);
    public static Vector3 colorWater = new Vector3(134,183,217);
    public static Vector3 colorThunder = new Vector3(223, 227, 115);

}

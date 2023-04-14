using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigConst
{
    //战斗与抽卡
    public static int maxCardHand = 5;    //     手牌最大
    public static int maxCardTake = 4;    //     take最大
    public static float cardwait = 1.25f;//    抽一张牌需要等待
    public static float cardtime_dealtoshow = 0.5f; //  抽卡到展示
    public static float cardtime_showstay = 0.7f;   //展示时常
    public static float cardtime_showtohand = 0.4f; //展示完毕到手上
    public static float cardtime_takeOnOff = 0.4f; //选中到台面或台面回手
    public static float cardtime_refshMove = 0.25f; //刷新卡速度

    public static int dealcard_constID = 2;    //魔法糖（抽卡）默认id
    public static int dealcard_useUp = 3;    //疲劳抽卡 默认id
}

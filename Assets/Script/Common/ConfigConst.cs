using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConfigConst
{
    //战斗与抽卡
    public const int maxCardHand = 6;    //     手牌最大
    public const int maxCardTake = 4;    //     take最大
    public const float cardwait = 1.25f;//    抽一张牌需要等待
    public const float cardtime_dealtoshow = 0.5f; //  抽卡到展示
    public const float cardtime_showstay = 0.7f;   //展示时常
    public const float cardtime_showtohand = 0.4f; //展示完毕到手上
    public const float cardtime_refshMove = 0.25f; //刷新卡速度
    public const float cardtime_effectShow = 1.25f; //卡生效放大展示时间
    public const float cardtime_effectMove = 0.4f; //攻击粒子飞行时间
    public const float cardtime_effectMoveSlow = 0.6f; //攻击粒子慢飞行时间
    public const float cardtime_addition = 0.8f; //填卡

    public const int dealcard_blessed = 91;    //仙灵祝福 默认id
    public const int dealcard_blessGod = 92;    //神灵祝福 默认id

    public const int QUEST_MAX= 6;    //任务上限
    public const int SPRITE_LEVEL_MAX = 50; //精灵等级上限
    public const int SQUARE_MAX = 5;    //地区上限
    public const int UPGRADE_MAX = 5;   //磨坊等级上限
    public const string NOIMG_ICON = "defaulticon";    //空图默认图片
    public const int markOpenTime = 1;
    public const int markEndTime = 19;  //商店起始时间
    public const int markMaxCount = 80; //商店容量
    public const int currencyID = 1;        //货币id
    public const int cardGeneralID = 50;        //通用卡牌id
    public const int explorMoveSpend = 2;   //移动消耗
    public const int explorBossAndSpecialSpend = 4;
    public const int explorExitBoxMaxMoneyID = 221; //小包银币
    public const int explorWayID = 0;   //森林小径id

    public static Vector3 colorForest = new Vector3(164,210,146);
    public static Vector3 colorFire = new Vector3(215,129,125);
    public static Vector3 colorWater = new Vector3(134,183,217);
    public static Vector3 colorThunder = new Vector3(223, 227, 115);


    public const int Quest_SS = 10000;
    public const int Quest_S = 6000;
    public const int Quest_A = 3000;
    public const int Quest_B = 1600;
    public const int Quest_C = 900;
    public const int Quest_D = 400;
    public const int Quest_E = 250;

    //词条描述
    public static string entryname(string str)
    {
        switch (str)
        {
            case "2":return "先制\n本张牌执行结束后立即执行下一张牌。";
            case "3":return "连击\n若自己的上一张牌造成过伤害，则触发连击词条的效果。";
            case "4":return "重击\n若生效时对方没有待生效牌，则出发重击词条效果。";
            case "7":return "诅咒\n反制对方下一张待生效的牌。";
            case "9":return "魔力枯竭\n在对对手造成高额伤害的同时自身也收到少量伤害。";
            case "12":return "屏障\n免疫对手下一张牌造成的伤害。";
            case "13":return "秘术\n若本回合只有这一张牌存在，则触发秘术词条效果。";
            case "14":return "真言\n该牌生效时免疫对方的诅咒效果，若成功免疫则触发真言后续词条效果。";
            case "16":return "破甲\n该牌生效时不对护甲造成伤害，直接伤害生命值。";
            case "28":return "奥术强化\n奥术强化作为一种常驻状态，当使用含有奥术强化词条的卡时消耗该状态获得强化效果。";
            default:return "";
        }
    }
    //卡牌类型
    public static string getCardType(CardType1 stype)
    {
        switch (stype)
        {
            case CardType1.take:return "基础卡";
            case CardType1.untaken:return "魔法卡";
            case CardType1.condition: return "限制卡";
            default:return "";
        }
    }
}

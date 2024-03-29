﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundData : BaseData
{
    public CardEntity entity;
    public t_DataCard _card;
    public SpriteData sprite;
    public bool isplayer;
    //效果  造成伤害等计算一下
    public int hitdef;  //对盾伤害  不用来计算伤害，显示用
    public int hitnum;      //伤害mix
    public int hitType;     //01234伤害属性
    public List<int> morehit = new List<int>();
    public int hit_addition;    //伤害补正

    public int hitselfnum;      //打自己
    public int recovernum;      //恢复
    public int defnum;      //防御
    public int dealnum;     //抽
    public int continuousnum;       //连击数
    public int toDefen;     //对甲
    public bool isbroken;    //穿甲     
    public bool isdefend;       //被防护
    public bool isCounter;  //被反制
    public bool isdecounter;    //反反制成功

    public bool havePower;  //秘术生效
    public bool haveCounter;    //反制效果标记

    public bool etch;   //腐蚀特效
    public List<int> gift = new List<int>();
    public int addition;    //洗入卡
    public int extraLimit;  //能量上限
    public bool isBasic;        //卡组卡？
}

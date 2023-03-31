using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData:BaseData
{
    public int id;
    public string sname;
    public string sDescribe;
    public CardType1 type1; //卡面类型
    public CardType2 type2; //效果类型
    public CardLimitType limit;
    public int damage1;
    public int damage2;
    public int damage3;
    public int cost;
}

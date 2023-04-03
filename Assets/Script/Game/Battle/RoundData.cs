using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundData : BaseData
{
    public t_DataCard.t_data _card;
    public bool isplayer;
    //效果  造成伤害等计算一下
    public int hitnum;
    public int recovernum;
    public int continuousnum;
    public bool isCounter;  //被反制
}

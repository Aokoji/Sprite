using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_UIPrefab
{
    none,
    Loading,
    Tips1,  //全屏 自动消失 文字提示
    Tips2,  //全屏 提示弹板
    Tips3,  //非全屏，简单提示文本，自动消失无回调
    BattlePanel,
    StartPanel,
    CardsetPanel,
    MainPanel,
    MillPanel,
    WorkshopPanel,
    ExplorPanel,    //探索选框
    TravelPanel,   //委托
    FarmPanel,
    SpringPanel,

    TravelSpriteMessagePanel,

    //ui
    cardHand,
    cardShow,
}
public enum E_Particle
{
    particle_boom,
    particle_hit,
    particle_move,
    particle_movefire,

    particle_chooseCardSet,
    particle_chooseCardBar,

}

public enum A_AtlasNames
{
    atlasImg1, 
    itemsIcon,
}

public enum B_BundleNames
{
    nprefab,
    natlas,
    ndataasset,
}

public enum S_SaverNames
{
    pdata,  //player save
    entrust,
}
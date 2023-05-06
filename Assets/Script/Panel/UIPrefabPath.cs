using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_UIPrefab
{
    none,
    Loading,
    Tips1,
    Tips2,
    BattlePanel,
    StartPanel,
    CardsetPanel,
    MainPanel,
    MillPanel,
    WorkshopPanel,
    ExplorPanel,    //探索选框
    EntrustPanel,   //委托
    FarmPanel,
    SpringPanel,

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
}

public enum A_AtlasNames
{
    atlasImg1,
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
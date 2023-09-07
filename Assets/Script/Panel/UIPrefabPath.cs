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
    Tips4,  //复杂奖励提示 自动消失无回调
    Tips5,  //复杂弹板奖励提示，仅确定
    ItemExplain,    //物品提示解释详情
    BattlePanel,
    StartPanel,
    CardsetPanel,
    MainPanel,
    MillPanel,
    MillAdditionPanel,  //添加生产物 bool ,int ,int   三参  ismater2,生产id,数量
    MillUpgradePanel,   //升级磨坊
    SpriteWorkPanel,    //int 派遣枚举
    WorkshopPanel,      //工坊
    TravelPanel,   //委托
    TravelBackQuestPanel,       //旅行回来领委托       QuestData 单参数
    TravelMoreMessagePanel, //委托详细信息        QuestData，bool  双参
    TravelSpriteMessagePanel,   //派遣精灵详情
    MarkPanel,  //商人
    MarkSalePanel,  //卖背包东西     int 卖上限,
    WareHousePanel,     //仓库
    ExplorPanel,    //探索选框
    ExplorOfferMorePanel,       //OfferData
    ExplorMovingPanel,      //探索地图
    ExplorBattleMessPanel,      //准备战斗
    ExplorGatherMessPanel,
    ExplorMovingPackage,    //探险背包
    ExplorBagSetPanel,  //准备背包
    SpriteMessagePanel,     //妖精详情
    SpriteCheckPanel,       //选择妖精小界面（上阵，使用物品
    FarmPanel,
    SpringPanel,        //魔力之泉
    CardMessageBar,     //卡牌详情  int 卡id

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
    changeSprite,
    wholeImg,
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
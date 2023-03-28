using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl :Object
{
    private BattlePanel ui;
    public bool loadSuccess = false;

    #region 获取数据  加载的准备阶段
    public void newbattle()
    {
        getPlayerCardData();

        createPanel();
    }
    private void getPlayerCardData()
    {

    }
    private void createPanel()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.BattlePanel, loadComplete);
    }
    private void loadComplete()
    {
        loadSuccess = true;
        ui = PanelManager.Instance.PanelCur.gameObject.GetComponent<BattlePanel>();
        //ui.initData();
    }
    #endregion

    #region  游戏流程
    //流程，发牌5：操作 出牌，抽牌
    private int round;  //回合计数
    private Queue<CardData> willTake;
    public void StartRound()
    {
        //回合开始 发牌
        ui.dealCard(1);
    }


    //出牌结束 开始统计回合
    private void settleCard()
    {

    }
    private void runRoundCard()
    {

    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class ExplorBattleMessPanel : PanelTopBase
{
    public Button backMain;
    public Button giveup;   //放弃宝箱
    public Button takeBtn;  //或战斗

    ExplorMovingPanel.rankReward _data;
    public override void init()
    {
        base.init();
        _data = message[0] as ExplorMovingPanel.rankReward;
    }
    public override void registerEvent()
    {
        base.registerEvent();
        giveup.onClick.AddListener(clickGiveUpBtn);
        takeBtn.onClick.AddListener(clickTakeBtn);
    }

    void clickTakeBtn()
    {

    }
    void clickGiveUpBtn()
    {
        EventAction.Instance.TriggerAction(eventType.explorGoNextRank_B, true);
        PanelManager.Instance.DisposePanel();
    }
}

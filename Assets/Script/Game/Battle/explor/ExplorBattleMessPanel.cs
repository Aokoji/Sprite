using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class ExplorBattleMessPanel : PanelTopBase
{
    public Text title;
    public Image enemyIcon;
    public Text enemyName;
    public Text enemyHp;
    public Text enemyLevel; //难度
    public Button runBattle;   //放弃
    public Button takebattle;  //战斗

    ExplorMovingPanel.rankReward _data;
    public override void init()
    {
        base.init();
        _data = message[0] as ExplorMovingPanel.rankReward;
        battlePanel();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        runBattle.onClick.AddListener(clickRunBattle);
        takebattle.onClick.AddListener(clickBattleGo);
    }

    void battlePanel()
    {
        title.text = "战斗详情";
        var dat = Config_t_ActorMessage.getOne(_data.enemyID);
        enemyName.text = dat.sname;
        enemyIcon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), dat.titleIcon);
        enemyHp.text = dat.hpbase.ToString();
        enemyLevel.text = "难度："+dat.spritePower.ToString();
        if (_data.stype == explorIcon.boss)
        {
            //+++
        }
    }
    void clickRunBattle()
    {
        PanelManager.Instance.showTips2("确定放弃战斗返回哨站吗？", () =>
        {
            PanelManager.Instance.JumpPanelScene(E_UIPrefab.MainPanel, () =>
            {
                EventAction.Instance.TriggerAction(eventType.jumpMainExplor);
            });
        });
    }
    void clickBattleGo()
    {
        BattleManager.Instance.EnterBattle(_data.enemyID,_data.stype);
    }
}

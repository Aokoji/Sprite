using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PanelBase
{
    public Button startbtn;
    public Button editbtn;
    public Button testbtn;
    public override void registerEvent()
    {
        base.registerEvent();
        startbtn.onClick.AddListener(startgame);
        editbtn.onClick.AddListener(editgame);
        PanelManager.Instance.panelUnlock();
        testbtn.onClick.AddListener(testClick);
    }
    void testClick()
    {
        BattleManager.Instance.EnterBattle(Main.NextEnemy, true, explorIcon.battle);
    }

    private void startgame()
    {
        //BattleManager.Instance.EnterBattle();
        PanelManager.Instance.ChangePanel(E_UIPrefab.MainPanel);
    }
    private void editgame()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.CardsetPanel);
    }
    private void exitgame()
    {
        Application.Quit();
    }
}

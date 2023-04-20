using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PanelBase
{
    public Button startbtn;
    public Button editbtn;

    public override void registerEvent()
    {
        base.registerEvent();
        startbtn.onClick.AddListener(startgame);
        editbtn.onClick.AddListener(editgame);
    }

    private void startgame()
    {
        BattleManager.Instance.EnterBattle();
        //PanelManager.Instance.OpenPanel(E_UIPrefab.BattlePanel);
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

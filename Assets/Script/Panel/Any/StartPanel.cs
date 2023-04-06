using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PanelBase
{
    public Button startbtn;
    public Button exitbtn;

    public override void registerEvent()
    {
        base.registerEvent();
        startbtn.onClick.AddListener(startgame);
        exitbtn.onClick.AddListener(exitgame);
    }

    private void startgame()
    {

    }
    private void exitgame()
    {
        Application.Quit();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : PanelBase
{
    public Button mill;
    public Button workshop;
    public Button explor;   //探索
    public Button entrust;  //委托
    public Button farm;
    public Button spring;

    public override void registerEvent()
    {
        base.registerEvent();
        mill.onClick.AddListener(jumpmill);
        workshop.onClick.AddListener(jumpworkshop);
        explor.onClick.AddListener(jumpexplor);
        entrust.onClick.AddListener(jumpentrust);
        farm.onClick.AddListener(jumpfarm);
        spring.onClick.AddListener(jumpspring);
    }
    public override void init()
    {
        base.init();
        RunSingel.Instance.getBeiJingTime((tim) => { Debug.Log(tim.ToString()); });
    }

    #region  onclick
    void jumpmill(){ PanelManager.Instance.OpenPanel(E_UIPrefab.MillPanel);}
    void jumpworkshop(){ PanelManager.Instance.OpenPanel(E_UIPrefab.MillPanel);}
    void jumpexplor(){ PanelManager.Instance.OpenPanel(E_UIPrefab.MillPanel);}
    void jumpentrust(){ PanelManager.Instance.OpenPanel(E_UIPrefab.MillPanel);}
    void jumpfarm(){ PanelManager.Instance.ChangePanel(E_UIPrefab.MillPanel);}
    void jumpspring(){ PanelManager.Instance.ChangePanel(E_UIPrefab.MillPanel);}
    #endregion
}

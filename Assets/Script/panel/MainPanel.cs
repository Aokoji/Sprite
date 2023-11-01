using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class MainPanel : PanelBase
{
    public Button mill;
    public Button workshop;
    public Button explor;   //探索
    public Button entrust;  //委托
    public Button mark; //商人
    public Button farm;
    public Button spring;
    public Button ware; //仓库

    //休息（废弃磨坊），探索（哨站），制作（工坊），常驻商店，仓库

    public override void registerEvent()
    {
        base.registerEvent();
        mill.onClick.AddListener(jumpmill);
        workshop.onClick.AddListener(jumpworkshop);
        explor.onClick.AddListener(jumpexplor);
        entrust.onClick.AddListener(jumpentrust);
        farm.onClick.AddListener(jumpfarm);
        spring.onClick.AddListener(jumpspring);
        mark.onClick.AddListener(jumpMark);
        ware.onClick.AddListener(jumpWare);
        EventAction.Instance.AddEventGather(eventType.jumpMainExplor, jumpexplor);
    }
    public override void unregisterEvent()
    {
        base.unregisterEvent();
        EventAction.Instance.RemoveAction(eventType.jumpMainExplor, jumpexplor);
    }
    public override void init()
    {
        //RunSingel.Instance.getBeiJingTime((tim) => { Debug.Log(tim.ToString()); });
    }

    #region  onclick
    void jumpmill(){ PanelManager.Instance.OpenPanel(E_UIPrefab.SpriteMessagePanel);}
    void jumpworkshop(){ PanelManager.Instance.OpenPanel(E_UIPrefab.WorkshopPanel);}
    void jumpexplor(){
        PanelManager.Instance.OpenPanel(E_UIPrefab.ExplorPanel);
    }
    void jumpentrust(){ PanelManager.Instance.OpenPanel(E_UIPrefab.TravelPanel);}
    void jumpfarm(){ PanelManager.Instance.ChangePanel(E_UIPrefab.MillPanel);}
    void jumpspring(){ PanelManager.Instance.ChangePanel(E_UIPrefab.MillPanel);}
    void jumpMark(){ PanelManager.Instance.OpenPanel(E_UIPrefab.MarkPanel);}
    void jumpWare(){ PanelManager.Instance.OpenPanel(E_UIPrefab.WareHousePanel);}
    #endregion
}

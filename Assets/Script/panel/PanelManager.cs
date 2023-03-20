using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class PanelManager : CSingel<PanelManager>
{
    private E_UIPrefab curEnmu;
    private PanelBase curPanel;
    public PanelBase PanelCur { get { return curPanel; } }

    private Stack<PanelBase> panelStack = new Stack<PanelBase>();
    private string PANEL_PATH = "Assets/ui/panel/";

    public void init()
    {
        initEvent();
        loadingPanel();
    }
    private void initEvent()
    {
        //EventAction.Instance.AddEventGather<string>(eventType.changePanel_S, changePanel);
        //EventAction.Instance.AddEventGather(eventType.panelChangeLoadingComplete, changPanelWithLoadingComplete);
    }
    public Action loadingComplete;
    public void OpenPanel(E_UIPrefab pop,Action callback=null)
    {
        if (curEnmu == pop) return;
        curEnmu = pop;
        loadingComplete = callback;
        RunSingel.Instance.runTimer(enterPanel());
    }
    IEnumerator enterPanel()
    {
        //shut wait
        var  obj = AssetLoad.Instance.loadUIPrefab<GameObject>(PANEL_PATH , curEnmu.ToString());
        UnityEngine.Object.Instantiate(obj);
        PanelBase panel = obj.GetComponent<PanelBase>();
        curPanel = panel;
        while(panel==null)
            yield return null;
        //shut end
        ChangePanelComplete();
    }
    public void ChangePanelComplete()
    {
        panelStack.Push(curPanel);
        curPanel.init();
        loadingComplete?.Invoke();
        EventAction.Instance.TriggerAction(eventType.panelChangeLoadingComplete, curEnmu);
    }

    public string UI_PATH = "Assets/ui/";
    private string CUSTOM_UI_PATH = "Assets/ui/custom/";
    public UIBase LoadUI(E_UIPrefab pop,string custompath="")
    {
        var obj = AssetLoad.Instance.loadUIPrefab<GameObject>(string.IsNullOrEmpty(custompath)? CUSTOM_UI_PATH:custompath, pop.ToString());
        UnityEngine.Object.Instantiate(obj);
        UIBase ui = obj.GetComponent<UIBase>();
        return ui;
    }

    //  ----------------------- loading ----------------------
    private LoadingPanel loading;
    private string LOAD_PATH = "ui/common/";
    private void loadingPanel()
    {
        loading = AssetLoad.Instance.loadUIPrefab<LoadingPanel>(LOAD_PATH,E_UIPrefab.Loading.ToString());
    }
    public void LoadingShow(bool show)
    {
        if (null != loading) loading.Loading(show);
    }
}

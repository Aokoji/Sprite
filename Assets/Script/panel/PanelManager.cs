using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class PanelManager : CSingel<PanelManager>
{
    private E_UIPrefab curEnmu;
    private PanelBase curPanel;

    private Stack<PanelBase> panelStack = new Stack<PanelBase>();
    private string PANEL_PATH = "ui/panel/";

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

    public void OpenPanel(E_UIPrefab pop)
    {
        if (curEnmu == pop) return;
        curEnmu = pop;
        RunSingel.Instance.runTimer(enterPanel());
    }
    IEnumerator enterPanel()
    {
        //shut wait
        PanelBase panel = AssetLoad.Instance.loadUIPrefab<PanelBase>(PANEL_PATH , curEnmu.ToString());
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
        EventAction.Instance.TriggerAction(eventType.panelChangeLoadingComplete, curEnmu);
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

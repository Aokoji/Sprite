﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class PanelManager : CSingel<PanelManager>
{
    public string curEnmu { get; private set; }
    private PanelBase curPanel;
    public PanelBase PanelCur { get { return curPanel; } }

    private Stack<PanelBase> panelStack = new Stack<PanelBase>();
    private string PANEL_PATH = "ui/panel/";
    Transform basePanel;
    GameObject shadow;
    public GameObject maincanvas;

    public void init()
    {
        maincanvas = GameObject.Find("Canvas");
        basePanel = maincanvas.transform.Find("BasePanel");
        shadow = maincanvas.transform.Find("shadow").gameObject;
        panelUnlock();
        initEvent();
        loadSupplyPop();
    }
    private void initEvent()
    {
        //EventAction.Instance.AddEventGather<string>(eventType.changePanel_S, changePanel);
        //EventAction.Instance.AddEventGather(eventType.panelChangeLoadingComplete, changPanelWithLoadingComplete);
    }
    object[] objdata;
    public void OpenPanel(E_UIPrefab pop, object[] obj = null)
    {
        if (curEnmu == pop.ToString()) return;
        curEnmu = pop.ToString();
        objdata = obj;
        RunSingel.Instance.runTimer(enterPanel());
    }
    public void ChangePanel(E_UIPrefab pop, object[] obj = null)
    {
        if (panelStack.Count <= 0)
        {
            Debug.LogError("退出界面错误，退栈为空");
            return;
        }
        panelStack.Pop();
        curPanel.Dispose();
        curEnmu = "";
        curPanel = null;
        OpenPanel(pop, obj);
    }
    public void ChangeScenePanel(E_UIPrefab pop, object[] obj = null)
    {
        while (panelStack.Count > 0)
        {
            panelStack.Pop().Dispose();
        }
        curEnmu = "";
        curPanel = null;
        OpenPanel(pop, obj);
    }
    IEnumerator enterPanel()
    {
        //shut wait
        var  obj = AssetManager.loadAsset<GameObject>(PANEL_PATH + curEnmu.ToString());
        //var obj = Resources.Load<GameObject>(curEnmu.ToString());
        var t = UnityEngine.Object.Instantiate(obj);
        t.transform.SetParent(basePanel, false);
        PanelBase panel = t.GetComponent<PanelBase>();
        //if (curPanel != null) curPanel.gameObject.SetActive(false);
        curPanel = panel;
        curPanel.PanelName = curEnmu;
        while(panel==null)
            yield return null;
        ChangePanelComplete();
    }
    public void ChangePanelComplete()
    {
        panelStack.Push(curPanel);
        curPanel.init(objdata);
        EventAction.Instance.TriggerAction(eventType.panelChangeLoadingComplete);
        jumpaction?.Invoke();
        jumpaction = null;
    }
    Action jumpaction;
    public void JumpPanelScene(E_UIPrefab panel,Action callback)
    {
        jumpaction = callback;
        ChangeScenePanel(panel);
    }
    public void RefreshCurPanel()
    {
        if (curPanel != null)
        {
            curPanel.reshow();
        }
    }
    public void DisposePanel()
    {
        if (panelStack.Count <= 0)
        {
            Debug.LogError("退出界面错误，退栈为空");
            return;
        }
        panelStack.Pop();
        curPanel.Dispose();
        if (panelStack.Count > 0)
        {
            curPanel = panelStack.Peek();
            curEnmu = curPanel.PanelName;
            curPanel.gameObject.SetActive(true);
            curPanel.reshow();
        }
        else
        {
            curEnmu = "";
            curPanel = null;
        }
    }

    public UIBase LoadUI(E_UIPrefab pop,string custompath,Transform parent=null)
    {
        var obj = AssetManager.loadAsset<GameObject>(custompath+ pop.ToString());
        var entity = UnityEngine.Object.Instantiate(obj);
        if (null != parent)
        {
            entity.transform.SetParent(parent);
            entity.transform.localScale = Vector3.one;
        }
        UIBase ui = entity.GetComponent<UIBase>();
        return ui;
    }

    //屏蔽点击
    public void panelLock() { shadow.SetActive(true); }
    public void panelUnlock() { shadow.SetActive(false); }

    private GameObject commonParent;
    private void loadSupplyPop()
    {
        commonParent = new GameObject("commonPop");
        commonParent.transform.SetParent(maincanvas.transform);
        commonParent.transform.localPosition = Vector3.zero;
        commonParent.transform.localScale = Vector3.one;
        loadingPanel();
        loadTips1Panel();
        loadTips2Panel();
        loadTips5Panel();
        loadTips3Panel();
        loadTips4Panel();
    }
    //  ----------------------- loading ----------------------
    private LoadingPanel loading;
    private string COMMON_PATH = "ui/common/";
    private void loadingPanel()
    {
        var entity = AssetManager.loadAsset< LoadingPanel>(COMMON_PATH+E_UIPrefab.Loading.ToString());
        loading = UnityEngine.Object.Instantiate(entity);
        loading.transform.SetParent(commonParent.transform);
        loading.transform.localScale = Vector3.one;
        loading.gameObject.SetActive(false);
    }
    public void LoadingShow(bool show,bool isconect=false)
    {
        if (null != loading) loading.Loading(show, isconect);
    }

    #region tip
    //------------------------ tips ----------------------------
    private TipsBase tip1;
    private TipsBase tip2;
    private TipsBase tip3;
    private TipsBase tip4;
    private TipsBase tip5;
    private void loadTips1Panel()
    {
        var entity = AssetManager.loadAsset<GameObject>(COMMON_PATH+ E_UIPrefab.Tips1.ToString());
        tip1 = UnityEngine.Object.Instantiate(entity).GetComponent<TipsBase>();
        tip1.transform.SetParent(commonParent.transform);
        tip1.transform.localScale = Vector3.one;
        tip1.gameObject.SetActive(false);
    }
    private void loadTips2Panel()
    {
        var entity = AssetManager.loadAsset<GameObject>(COMMON_PATH+ E_UIPrefab.Tips2.ToString());
        tip2 = UnityEngine.Object.Instantiate(entity).GetComponent<TipsBase>();
        tip2.transform.SetParent(commonParent.transform);
        tip2.transform.localScale = Vector3.one;
        tip2.gameObject.SetActive(false);
    }
    private void loadTips3Panel()
    {
        var entity = AssetManager.loadAsset<GameObject>(COMMON_PATH + E_UIPrefab.Tips3.ToString());
        tip3 = UnityEngine.Object.Instantiate(entity).GetComponent<TipsBase>();
        tip3.transform.SetParent(commonParent.transform);
        tip3.transform.localScale = Vector3.one;
        tip3.transform.SetAsLastSibling();
        tip3.gameObject.SetActive(false);
    }
    private void loadTips4Panel()
    {
        var entity = AssetManager.loadAsset<GameObject>(COMMON_PATH + E_UIPrefab.Tips4.ToString());
        tip4 = UnityEngine.Object.Instantiate(entity).GetComponent<TipsBase>();
        tip4.transform.SetParent(commonParent.transform);
        tip4.transform.localScale = Vector3.one;
        tip4.gameObject.SetActive(false);
    }
    private void loadTips5Panel()
    {
        var entity = AssetManager.loadAsset<GameObject>(COMMON_PATH + E_UIPrefab.Tips5.ToString());
        tip5 = UnityEngine.Object.Instantiate(entity).GetComponent<TipsBase>();
        tip5.transform.SetParent(commonParent.transform);
        tip5.transform.localScale = Vector3.one;
        tip5.gameObject.SetActive(false);
    }
    /// <summary>
    /// 占屏提示
    /// </summary>
    public void showTips1(string str,Action callback=null)
    {
        tip1.init(str, callback);
        tip1.play();
    }
    /// <summary>
    /// 是否提示板
    /// </summary>
    public void showTips2(string str,Action callback, Action callback2=null)
    {
        tip2.init(str, callback, callback2);
        tip2.setString2("");
        tip2.play();
    }
    public void showTips2(string str,string str2, Action callback, Action callback2 = null)
    {
        tip2.init(str, callback, callback2);
        tip2.setString2(str2);
        tip2.play();
    }
    /// <summary>
    /// 浅提示
    /// </summary>
    public void showTips3(string str)
    {
        tip3.init(str);
        tip3.play();
    }
    /// <summary>
    /// 获得物品提示
    /// </summary>
    public void showTips4(List<ItemData> items)
    {
        tip4.init(items);
        tip4.play();
    }
    public void showTips5(string title,List<ItemData> items,Action callback)
    {
        tip5.init(title,items, callback);
        tip5.play();
    }
    public void showTips5(ItemData items)
    {
        tip5.init(items);
        tip5.play();
    }
    public void showTips5(string title, string des, Action callback)
    {
        tip5.init(title, new List<ItemData>(), callback);
        tip5.setString2(des);
        tip5.play();
    }
    public void showTips6(string str, Action callback = null)
    {
        tip1.init(str, callback);
        tip1.play();
    }
    #endregion

    public void showComItemTip(int id,Vector3 pos)
    {
        var entity = AssetManager.loadAsset<GameObject>(COMMON_PATH + E_UIPrefab.ItemExplain.ToString());
        var script = UnityEngine.Object.Instantiate(entity).GetComponent<ItemExplain>();
        script.transform.SetParent(commonParent.transform);
        script.transform.localScale = Vector3.one;
        script.initShow(id, pos);
    }
}

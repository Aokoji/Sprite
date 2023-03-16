using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using common_word;
using common_event;

public class PanelControl
{
    public PanelControl(GameObject pos)
    {
        bronpos = pos.transform;
        init();
    }
    private Transform bronpos;
    private PanelBase curPanel;
    private string panelPath = "panel/";

    public void init()
    {
        initEvent();
    }
    private void initEvent()
    {
        ManageEvent.addListener<string>(eventName.panelChange, changePanel);
        ManageEvent.addListener(eventName.panelChangeLoadingComplete, changPanelWithLoadingComplete);
    }

    public void changePanel(string sname)
    {
        if (curPanel != null && sname.Equals(curPanel.PanelName))
        {
            LogTool.LogWarn("界面切换操作重复:" + sname,warnLevel.warn1);
            return;
        }
        if (curPanel != null)
        {
            curPanel.GetComponent<PanelBase>().OnExit();
            ObjectUtil.Destroy(curPanel.gameObject);
        }
        var item = PrefabLoad.loadPrefab(panelPath + sname);
        curPanel = item.GetComponent<PanelBase>();
        curPanel.init();
        item.transform.SetParent(bronpos, false);
    }
    private void changPanelWithLoadingComplete()
    {
        if (curPanel != null)
        {
            curPanel.GetComponent<PanelBase>().initAfterLoading();
        }
        else
        {
            Debug.LogError("界面加载完成事件调空！");
        }
    }
}

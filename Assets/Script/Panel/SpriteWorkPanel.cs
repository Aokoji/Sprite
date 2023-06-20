using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteWorkPanel : PanelTopBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Button backBtn;

    e_workSquare square;

    public override void init()
    {
        base.init();
        square = (e_workSquare)(int)message[0];
        StartCoroutine(initScrollData());
    }
    public override void registerEvent()
    {
        base.registerEvent();
        backBtn.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }

    IEnumerator initScrollData()
    {
        scroll.initConfig(455, 100, clone.gameObject);
        foreach (var item in PlayerManager.Instance.spriteList)
        {
            var obj = scroll.addItemDefault().GetComponent<TravelSpriteMessageBar>();
            obj.setData(item.Value, square);
            obj.initAction(goWorkAction);
            obj.gameObject.SetActive(true);
        }
        yield return null;
    }

    void goWorkAction(int id)
    {
        //不足判定
        var spdata = PlayerManager.Instance.getSpriteData(id);
        if (spdata.phy_cur<=0)
        {
            PanelManager.Instance.showTips3("妖精体力不足");
            return;
        }
        if (spdata.istraveling)
        {
            PanelManager.Instance.showTips3("妖精正在旅行中");
            return;
        }
        if (spdata.isworking)
        {
            PanelManager.Instance.showTips3("妖精正在工作中");
            return;
        }
        WorkManager.Instance.WorkStart(square, id);
        PanelManager.Instance.DisposePanel();
    }
}

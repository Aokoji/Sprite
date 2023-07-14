using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class TravelPanel : PanelTopBase
{
    public Button gotravel;
    public TravelBarMessage[] quests;
    public Button backBtn;

    bool travelAllow;

    public override void init()
    {
        initData();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        //EventAction.Instance.AddEventGather(eventType.spriteTravelComplete_I, travelComplete);
        //gotravel.onClick.AddListener(() => { TravelManager.Instance.goTravel(PlayerManager.Instance.cursprite.id, 2); });
        gotravel.onClick.AddListener(gotravelClick);
        backBtn.onClick.AddListener(PanelManager.Instance.DisposePanel);
        EventAction.Instance.AddEventGather(eventType.spriteTravelBackRefresh, refreshTravels);
    }
    public override void unregisterEvent()
    {
        base.unregisterEvent();
        EventAction.Instance.RemoveAction(eventType.spriteTravelBackRefresh, refreshTravels);
    }
    public void initData()
    {
        travelAllow = true;
        refreshTravels();
    }
    public override void reshow()
    {
        base.reshow();
        refreshTravels();
    }
    void refreshTravels()
    {
        for (int i = 0; i < ConfigConst.QUEST_MAX; i++)
            quests[i].gameObject.SetActive(false);
        RunSingel.Instance.getBeiJingTime((result) =>
        {
            var selfquset = TravelManager.Instance._data.quest;
            foreach(var item in selfquset)
            {
                quests[item.pagePos].reset(item, result);
                quests[item.pagePos].gameObject.SetActive(true);
            }
            travelCheck(selfquset.Count<=ConfigConst.QUEST_MAX);
        });
    }
    void travelCheck(bool allow)
    {
        travelAllow = allow;
        if(allow)
            gotravel.GetComponent<Image>().color = Color.white;
        else
            gotravel.GetComponent<Image>().color = Color.gray;
    }
    void gotravelClick()
    {
        if (!travelAllow)
        {
            PanelManager.Instance.showTips3("委托板已满");
            return;
        }
        PanelManager.Instance.OpenPanel(E_UIPrefab.TravelSpriteMessagePanel);
    }
    //派遣成功回调
    void travelComplete(int id)
    {
        var aa = TravelManager.Instance._data;
        /*Debug.Log(aa.traveling.Count);
        if (aa.traveling.Count > 0)
        {
            Debug.Log(aa.traveling[0].questID + "      " + aa.traveling[0].finish);
        }*/
    }
}

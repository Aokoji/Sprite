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
        base.init();
        travelAllow = true;
        initData();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        EventAction.Instance.AddEventGather(eventType.spriteTravelComplete, travelComplete);
        //gotravel.onClick.AddListener(() => { TravelManager.Instance.goTravel(PlayerManager.Instance.cursprite.id, 2); });
        gotravel.onClick.AddListener(gotravelClick);
        backBtn.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }
    public void initData()
    {

        refreshTravels();
    }
    void refreshTravels()
    {
        RunSingel.Instance.getBeiJingTime((result) =>
        {
            var selfquset = TravelManager.Instance._data.quest;
            for(int i = 0; i < ConfigConst.QUEST_MAX; i++)
                quests[i].gameObject.SetActive(false);
            foreach(var item in selfquset)
            {
                quests[item.pagePos].reset(selfquset[i], result);
                quests[item.pagePos].gameObject.SetActive(true);
            }
            travelCheck(selfquset.Count>=ConfigConst.QUEST_MAX);
        });
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
    void travelCheck(bool allow)
    {
        travelAllow = allow;
        if(allow)
            gotravel.GetComponent<Image>().color = Color.white;
        else
            gotravel.GetComponent<Image>().color = Color.gray;
    }

    //派遣成功回调
    void travelComplete()
    {
        var aa = TravelManager.Instance._data;
        /*Debug.Log(aa.traveling.Count);
        if (aa.traveling.Count > 0)
        {
            Debug.Log(aa.traveling[0].questID + "      " + aa.traveling[0].finish);
        }*/
    }
}

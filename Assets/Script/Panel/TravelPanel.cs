using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class TravelPanel : PanelBase
{
    public Button test;

    public override void init()
    {
        base.init();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        EventAction.Instance.AddEventGather(eventType.spriteTravelComplete, travelComplete);
        test.onClick.AddListener(() => { TravelManager.Instance.goTravel(PlayerManager.Instance.cursprite.id, 2); });
    }

    void travelComplete()
    {
        var aa = TravelManager.Instance._data;
        Debug.Log(aa.traveling.Count);
        if (aa.traveling.Count > 0)
        {
            Debug.Log(aa.traveling[0].questID + "      " + aa.traveling[0].finish);
        }
    }
}

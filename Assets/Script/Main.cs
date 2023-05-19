using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.initManager();
        PanelManager.Instance.OpenPanel(E_UIPrefab.StartPanel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //PanelManager.Instance.OpenPanel(E_UIPrefab.CardsetPanel);
            var d = new QuestData();
            d.questID = 1;
            d.spID = 1;
            d.spFinish = DateTime.Now.AddMinutes(-10);
            d.endTime = DateTime.Now.AddMinutes(25);
            d.takeItem.Add(1);
            d.takeItem.Add(1);
            d.isGet = false;
            PlayerManager.Instance.getplayerTravel().quest.Add(d);
        }
    }
}

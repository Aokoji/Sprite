﻿using System.Collections;
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
            PlayerManager.Instance.getplayerTravel().quest.Add(d);
        }
    }
}

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
            PlayerManager.Instance.addItems(6, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerManager.Instance.addItems(49, 1);
        }
    }
}

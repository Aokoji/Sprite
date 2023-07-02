using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Main : MonoBehaviour
{
    public bool isClearData;
    void Start()
    {
#if UNITY_EDITOR
        if (isClearData)
            File.Delete(Application.persistentDataPath + "/" + S_SaverNames.pdata.ToString());
#endif
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

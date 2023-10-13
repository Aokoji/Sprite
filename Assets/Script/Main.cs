using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Main : MonoBehaviour
{
    public bool isClearData;
    public int dayMapID;
    public bool allCardOpen;
    public int nextEnemy;

    public static int NextEnemy;
    public static bool AllCardOpen;
    public static int DayMapID;
    void Start()
    {
#if UNITY_EDITOR
        if (isClearData)
            File.Delete(Application.persistentDataPath + "/" + S_SaverNames.pdata.ToString());
        DayMapID = dayMapID;
        AllCardOpen = allCardOpen;
        NextEnemy = nextEnemy;
#endif
        GameManager.Instance.initManager();
        PanelManager.Instance.OpenPanel(E_UIPrefab.StartPanel);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerManager.Instance.addItems(8, 10);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerManager.Instance.addMagicBookNosave(53);
            PlayerManager.Instance.addItems(53, 1);
        }
    }
}

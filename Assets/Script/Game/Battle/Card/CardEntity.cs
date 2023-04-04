using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEntity : UIBase
{
    public Image bg;
    public Image desbg;
    public Text sname;
    public Text descirbe;
    public GameObject body;

    //单张卡
    public t_DataCard.t_data _data;
    public Action<CardEntity> onChoose;
    public bool isStaying;  //准备出
    public bool clickAllow;

    public void initData(t_DataCard.t_data data)
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        _data = data;
        GetComponent<Button>().onClick.AddListener(onchoose);
        clickAllow = true;
    }
    private void onchoose()
    {
        if (!clickAllow) return;
        onChoose?.Invoke(this);
    }
}

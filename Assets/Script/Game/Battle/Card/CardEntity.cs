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
    public GameObject backBG;

    //单张卡
    public t_DataCard _data;
    public Action<CardEntity> onChoose;
    public bool isStaying;  //准备出
    public bool isback;//卡背
    public bool clickAllow;

    public void initData(t_DataCard data)
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        _data = data;
        refreshCard();
        GetComponent<Button>().onClick.AddListener(onchoose);
        clickAllow = true;
        isStaying = false;
    }
    private void refreshCard()
    {
        sname.text = _data.sname.ToString();
        descirbe.text = _data.sDescribe.ToString();
        backBG.SetActive(isback);
    }
    private void onchoose()
    {
        if (!clickAllow) return;
        onChoose?.Invoke(this);
    }
    public void turnCard()
    {
        isback = !isback;
        backBG.SetActive(isback);
    }
}

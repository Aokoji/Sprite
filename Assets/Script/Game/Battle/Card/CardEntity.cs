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
    public Text cost;

    public GameObject particleShow;
    public GameObject particleCounter;

    //单张卡
    public t_DataCard _data;
    public Action<CardEntity> onChoose;
    public bool isStaying;  //准备出
    public bool isback;//卡背
    public bool isdealcreate;   //抽卡创建出来的
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
        isdealcreate = false;
        particleShow.SetActive(false);
        particleCounter.SetActive(false);
    }
    private void refreshCard()
    {
        sname.text = _data.sname.ToString();
        descirbe.text = _data.sDescribe.ToString();
        backBG.SetActive(isback);
        cost.text = _data.cost.ToString();
        particleShow.SetActive(false);
        particleCounter.SetActive(false);
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

    public void playJustShowAnim(Action callback)
    {
        AnimationTool.playAnimation(gameObject, "cardShow", false, callback);
    }
    public void playJustHideAnim(Action callback)
    {
        AnimationTool.playAnimation(gameObject, "cardHide", false, callback);
    }
    public void playCounterAnim(Action callback)
    {
        AnimationTool.playAnimation(gameObject, "cardCounter", false, callback);
    }
}

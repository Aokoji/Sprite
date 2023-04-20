using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetEntity : UIBase
{
    public Image bg;
    public Image desbg;
    public Text sname;
    public Text descirbe;
    public GameObject body;
    public Text cost;

    public GameObject limit;
    public Text limitnum;

    //单张卡
    public t_DataCard _data;
    public Action<CardSetEntity> onChoose;
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
        //携带问题
        limit.SetActive(false);
    }
    private void refreshCard()
    {
        sname.text = _data.sname.ToString();
        descirbe.text = _data.sDescribe.ToString();
        cost.text = _data.cost.ToString();
        if (_data.limit == 0)
            bg.sprite = GetSprite("baseAtlas1", "buttonSquare_beige_pressed");
        else
            bg.sprite= GetSprite("baseAtlas1", "buttonSquare_blue_pressed");
    }
    private void onchoose()
    {
        if (!clickAllow) return;
        onChoose?.Invoke(this);
    }
    public void setOpen(bool isopen)
    {
        limit.SetActive(!isopen);
        clickAllow = isopen;
    }

}

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
    public CardData _data;
    public Action<CardEntity> onChoose;
    public bool isStaying;  //准备出

    public void initData(CardData data)
    {
        _data = data;
        GetComponent<Button>().onClick.AddListener(onchoose);
    }
    private void onchoose()
    {
        onChoose?.Invoke(this);
    }
}

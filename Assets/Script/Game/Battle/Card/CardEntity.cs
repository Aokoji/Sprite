using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEntity : UIBase
{
    public Image bg;
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
    public bool isextra;    //腐蚀用

    public void initData(t_DataCard data,bool extra=false)
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        body.transform.localScale = Vector3.one;
        GetComponent<CanvasGroup>().alpha=1;
        _data = data;
        isextra = extra;
        refreshCard();
        GetComponent<Button>().onClick.AddListener(onchoose);
        clickAllow = true;
        isStaying = false;
        isdealcreate = false;
        particleShow.SetActive(false);
        particleCounter.SetActive(false);

        bg.color = Color.white;
        bg.sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "card" + (int)_data.limit);
        /*
        if (_data.type1 == 0)
            bg.sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "card" + (int)_data.limit);
        else
            bg.sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "card_" + (int)_data.type1);
        */
        backBG.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "back");
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
        bg.gameObject.SetActive(!isback);
    }
    public void backCard()
    {
        isback = true;
        backBG.SetActive(true);
        bg.gameObject.SetActive(false);
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
    public void playNormalShowAnim(Action callback)
    {
        AnimationTool.playAnimation(gameObject, "cardNormalShow", false, callback);
    }
    public void playTurnFrontAnim(Action callback)
    {
        AnimationTool.playAnimation(gameObject, "cardTurnFront", false, callback);
    }
    public void playTurnBackAnim(Action callback)
    {
        AnimationTool.playAnimation(gameObject, "cardTurnBack", false, callback);
    }
}

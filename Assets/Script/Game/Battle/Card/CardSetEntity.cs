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
    public Text havenum;

    //单张
    public t_DataCard _data;
    public Action<CardSetEntity> onChoose;
    public bool clickAllow;

    private int count;  //拥有数量
    private int chooseNum;  //选择数量

    public void initData(t_DataCard data, int num = 0)
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        _data = data;
        refreshCard();
        GetComponent<Button>().onClick.AddListener(onchoose);
        clickAllow = true;
        limit.GetComponent<Image>().sprite= GetSprite(A_AtlasNames.atlasImg1.ToString(), "card" + (int)_data.limit);
        //携带问题
        limit.SetActive(false);
    }
    private void refreshCard(int num=-1)
    {
        sname.text = _data.sname.ToString();
        descirbe.text = _data.sDescribe.ToString();
        cost.text = _data.cost.ToString();

        bg.color = Color.white;
        bg.sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "card" + (int)_data.limit);
        /*
        if (_data.type1 == 0)
        {
            bg.sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "card" + (int)_data.limit);
        }
        else
        {
            bg.sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "card_" + (int)_data.type1);
        }*/

        if (num >= 0)
        {
            havenum.gameObject.SetActive(true);
            havenum.text = "x" + num;
        }
        else
            havenum.gameObject.SetActive(false);
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
    //

    public void chooseThisCard()
    {
        //可以点说明在open
        chooseNum++;
        if (_data.type1 != CardType1.take)
        {
            count--;
            if (count <= 0) setOpen(false);
        }

        
    }
    public void comeBackCard()
    {

    }
}

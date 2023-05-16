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
    public bool clickAllow;

    private int count;  //拥有数量
    private int chooseNum;  //选择数量
    private CardsetPanel ctrl;

    public void initData(t_DataCard data, int num,CardsetPanel ctl)
    {
        ctrl = ctl;
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
        count = num;
    }
    private void refreshCard()
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

        if (_data.type1!=CardType1.take)
        {
            havenum.gameObject.SetActive(true);
            havenum.text = "x" + count;
        }
        else
            havenum.gameObject.SetActive(false);
    }
    private void onchoose()
    {
        if (!clickAllow) return;
        if (ctrl.checkCardsFull())
        {
            PanelManager.Instance.showTips3("卡组已满");
            return;
        }
        //可以点说明在open
        chooseThisCard();
        ctrl.chooseCard(_data.id);
        ParticleManager.Instance.playEffect(E_Particle.particle_chooseCardSet, transform.position);
    }
    public void setOpen(bool isopen)
    {
        limit.SetActive(!isopen);
        clickAllow = isopen;
    }
    //

    public void chooseThisCard()
    {
        chooseNum++;
        //判断count
        if (_data.type1 != CardType1.take)
        {
            count--;
            if (count <= 0) setOpen(false);
        }
        //判断choose
        if (_data.limitcount <= chooseNum)
            setOpen(false);
        refreshCard();
    }
    public void comeBackCard()
    {
        chooseNum--;
        if (_data.type1 != CardType1.take)
        {
            count++;
        }
        setOpen(true);
        refreshCard();
    }
}

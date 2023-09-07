using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSetEntity : UIBase, IPointerDownHandler, IPointerUpHandler
{
    public Image bg;
    public Image desbg;
    public Text sname;
    public Text descirbe;
    public GameObject body;
    public Text cost;

    public GameObject limit;
    public Text havenum;

    public Text level;
    public Text cardtype;

    //单张
    public t_DataCard _data;
    public bool clickAllow;

    private int count;  //拥有数量
    private int chooseNum;  //选择数量

    float gaprun;
    bool isdown;

    Action<CardSetEntity> clickAction;
    public void initData(int id, int num,Action<CardSetEntity> clickaction)
    {
        clickAction = clickaction;
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
        _data = Config_t_DataCard.getOne(id);
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
        level.text = _data.level.ToString();
        cardtype.text = ConfigConst.getCardType(_data.type1);
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
        //可以点说明在open
        clickAction(this);
    }
    public void setOpen(bool isopen)
    {
        limit.SetActive(!isopen);
        clickAllow = isopen;
    }
    //
    public void showChooseParticle()
    {
        ParticleManager.Instance.playEffect(E_Particle.particle_chooseCardSet, transform.position);
    }
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


    private void Update()
    {
        if (isdown)
        {
            gaprun += Time.deltaTime;
            if (gaprun >= 1.2f)
            {
                //展示详情
                isdown = false;
                gaprun = 0;
                PanelManager.Instance.OpenPanel(E_UIPrefab.CardMessageBar, new object[] { _data.id });
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isdown = true;
        gaprun = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isdown = false;
        gaprun = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicEntity : UIBase ,IPointerDownHandler,IPointerUpHandler
{
    public Image bg;
    public Text sname;
    public Text count;
    public Text cost;
    public GameObject unuse;
    public GameObject onuse;
    public GameObject roundlimit;

    //单张卡
    t_DataCard _data;
    public Action<t_DataCard> onChoose;
    t_items _tdata;
    ItemData magicdata;
    bool beused;
    bool allowClick;
    bool roundUsed;
    public void setData(int id)
    {
        _tdata = Config_t_items.getOne(id);
        _data = null;
        onuse.SetActive(false);
        if (_tdata.type == ItemsType.magic)
        {
            _data = Config_t_DataCard.getOne(_tdata.connect);
            magicdata = PlayerManager.Instance.getMagicBook(_tdata.id);
            bg.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), _tdata.iconName);
            cost.text = _data.cost.ToString();
            cost.gameObject.SetActive(true);
            allowClick = true;
            unuse.SetActive(false);
            //刷新显示
            refreshCard();
        }
        else
        {
            cost.gameObject.SetActive(false);
            allowClick = false;
            unuse.SetActive(true);
        }
        sname.text = _data.sname.ToString();
        GetComponent<Button>().onClick.AddListener(onchoose);
    }
    public void resetAndSendCard()
    {//send意味着回合结束
        if (!beused) return;
        roundUsed = true;
        PlayerManager.Instance.useMagicBookOne(_tdata.id);
        resetCard();
    }
    public void refreshCard()
    {
        int curcount = magicdata.limitnum;
        if (curcount <= 0)
            count.color = Color.red;
        else
            count.color = Color.black;
        if (beused)
            curcount--;
        count.text = curcount + "/" + magicdata.limitMax;
        //一局限制魔法书
        if(magicdata.limitRound && roundUsed)
        {
            roundlimit.SetActive(true);
            allowClick = false;
        }
        else
        {
            roundlimit.SetActive(false);
            allowClick = true;
        }
    }
    public void resetCard()
    {//退还
        beused = false;
        onuse.SetActive(false);
        refreshCard();
    }
    private void onchoose()
    {
        if (!allowClick)
        {
            if(roundUsed)
                PanelManager.Instance.showTips3("本局已使用过，无法多次使用。");
            else
                PanelManager.Instance.showTips3("无法使用。");
            return;
        }
        if (beused) return;
        onuse.SetActive(true);
        beused = true;
        refreshCard();
        onChoose?.Invoke(_data);
    }

    public bool checkSelf(int id)
    {
        return _data != null && _data.id == id;
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

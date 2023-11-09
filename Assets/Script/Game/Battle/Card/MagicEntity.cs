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

    //单张卡
    public t_DataCard _data;
    public Action<t_DataCard> onChoose;
    t_items _tdata;
    t_Consumable _tConsum;
    bool beused;
    bool allowClick;
    public void setData(int id)
    {
        _tdata = Config_t_items.getOne(id);
        _data = null;
        _tConsum = null;
        if (_tdata.type == ItemsType.magic)
        {
            _data = Config_t_DataCard.getOne(_tdata.connect);
            _tConsum = Config_t_Consumable.getOne(id);
            bg.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), _tdata.iconName);
            cost.text = _data.cost.ToString();
            cost.gameObject.SetActive(true);
            allowClick = true;
            unuse.SetActive(false);
        }
        else
        {
            cost.gameObject.SetActive(false);
            allowClick = false;
            unuse.SetActive(true);
        }
        sname.text = _data.sname.ToString();
        refreshCard();
        GetComponent<Button>().onClick.AddListener(onchoose);
    }
    public void resetAndSendCard()
    {
        beused = false;
        int curcount = PlayerManager.Instance.getMagicBook(_tdata.id).limitnum;
        if (curcount <= 0)
            count.color = Color.red;
        else
            count.color = Color.black;
        count.text = curcount + "/" + _tConsum.takenum2;
    }
    public void refreshCard()
    {
        int curcount = PlayerManager.Instance.getMagicBook(_tdata.id).limitnum;
        if (curcount <= 0)
            count.color = Color.red;
        else
            count.color = Color.black;
        if (beused)
            curcount--;
        count.text = curcount + "/" + _tConsum.takenum2;
    }
    void chooseCard()
    {

    }
    private void onchoose()
    {
        if (!allowClick)
        {
            PanelManager.Instance.showTips3("无法使用");
            return;
        }
        if (beused) return;
        beused = true;
        refreshCard();
        onChoose?.Invoke(_data);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

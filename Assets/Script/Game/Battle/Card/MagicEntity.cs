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

    //单张卡
    public t_DataCard _data;
    public Action<t_DataCard> onChoose;
    t_items _tdata;
    bool allowClick;
    public void setData(int id)
    {
        _tdata = Config_t_items.getOne(id);
        _data = null;
        if (_tdata.type == ItemsType.magic)
        {
            _data = Config_t_DataCard.getOne(_tdata.connect);
            bg.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), _tdata.iconName);
            cost.text = _data.cost.ToString();
            cost.gameObject.SetActive(true);
            allowClick = true;
        }
        else
        {
            cost.gameObject.SetActive(false);
            allowClick = false;
        }
        refreshCard();
        GetComponent<Button>().onClick.AddListener(onchoose);
    }
    private void refreshCard()
    {
        sname.text = _data.sname.ToString();
    }
    private void onchoose()
    {
        //if (!clickAllow) return;
        onChoose?.Invoke(_data);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}

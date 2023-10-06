using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuffItem : UIBase, IPointerDownHandler, IPointerUpHandler
{
    public GameObject messobj;
    public Text buffdes;

    public Image buffimg;
    public Text buffText;
    public void OnPointerDown(PointerEventData eventData)
    {
        messobj.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        messobj.SetActive(false);
    }

    t_Buff _data;
    public void setData(int id,int num=0)
    {
        _data = Config_t_Buff.getOne(id);
        buffText.gameObject.SetActive(false);
        if (num != 0)
        {
            buffdes.text = string.Format(_data.sdes, num);
            buffText.text = num.ToString();
            buffText.gameObject.SetActive(true);
        }
        else
            buffdes.text = _data.sdes;
        messobj.SetActive(false);
        buffimg.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), _data.imgname);
        gameObject.SetActive(true);
    }

}

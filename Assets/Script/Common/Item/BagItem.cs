using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagItem : UIBase
{
    public Image icon;
    public Text count;


    t_items _data;
    public void initAction(Action<int> callback)
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(()=> { callback(_data.id); });
    }
    public void setData(int id,int num)
    {
        count.text = num.ToString();
        if (_data != null && _data.id==id)
        {
            return;
        }
        _data = Config_t_items.getOne(id);
        icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), _data.iconName);
    }
    public void setData(ItemData item)
    {
        //魔法书用这个
        count.text = item.num.ToString();
        if (_data != null && _data.id == item.id)
        {
            return;
        }
        _data = Config_t_items.getOne(item.id);
        icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), _data.iconName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ExplorQuickBagBar : UIBase
{
    public Button takeuse;
    public Image _icon;
    public Text _sname;
    public Text _typenum;
    private void Start()
    {
        takeuse.onClick.AddListener(clickuse);
    }
    public void setData(int id)
    {
        var item = Config_t_Consumable.getOne(id);
        var config = Config_t_items.getOne(id);
        _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), config.iconName);
        _sname.text = config.sname;
        if (item.type == ItemsType.magic)
        {
            //
            takeuse.gameObject.SetActive(false);
            _typenum.text = "魔法书";
        }
        else
        {
            //目前else只有 体力/生命 消耗品
            takeuse.gameObject.SetActive(true);
            _typenum.text = NormalCalculate.propertyName(item.takeid) +"+"+item.takenum;
        }
    }
    public Action onused;
    void clickuse()
    {
        onused?.Invoke();
    }
}

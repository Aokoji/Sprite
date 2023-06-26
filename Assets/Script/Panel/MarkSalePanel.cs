using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkSalePanel : PanelTopBase
{
    public Image _icon;
    public Text _count;
    public Text _valueContext;  //价值

    int residue;    //剩余空间
    public override void init()
    {
        base.init();
        residue = (int)message[0];
        var sdata = message[1] as ItemData;
        if (sdata == null)
        {
            //无传参，直接兑换银币
            _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(ConfigConst.currencyID).iconName);
        }
        else
        {
            //减去当前数量
            residue -= sdata.num;
            _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(sdata.id).iconName);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkItemBar : UIBase
{
    public Image icon;
    public Text count;
    public Text itemname;
    public Button exchange;
    public Text needPay;

    public Action<ItemData> showpanel;

    t_Business mark;
    bool canbuy;
    int pay;
    private void Start()
    {
        exchange.onClick.AddListener(clickexchange);
    }
    public void setData(int id)
    {
        mark = Config_t_Business.getOne(id);
        var item = Config_t_items.getOne(mark.itemid);
        icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), item.iconName);
        count.text = mark.salenum.ToString();
        itemname.text = item.sname;

        pay = item.pay * mark.salenum;
        needPay.text = pay.ToString();
        canbuy = PlayerManager.Instance.getItem(ConfigConst.currencyID)>=pay;
    }

    void clickexchange()
    {
        if (canbuy)
            PanelManager.Instance.showTips2("确认要购买这组物品吗？", () =>
            {
                PlayerManager.Instance.addItemsNosave(ConfigConst.currencyID, -pay);
                PlayerManager.Instance.addItems(mark.itemid, mark.salenum);
                PanelManager.Instance.RefreshCurPanel();
            });
        else
            PanelManager.Instance.showTips2("银币不足，是否使用物品进行交换？", () =>
            {
                showpanel?.Invoke(new ItemData(mark.itemid,mark.salenum));
            });
    }
    public int getCount() { return mark.salenum; }

}

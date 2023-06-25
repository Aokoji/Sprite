using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkPanel : PanelTopBase
{
    public MarkItemBar[] saleItems;
    public Button saleBtn;
    public GameObject noneObj;
    public Text businessBag;    //商人容量
    public Button bussman;
    //商人规则：每日10-17出现，交互随机对话，仓库容量总80，售卖物品容量30，槽位上限6，
    //售卖规则      1 2 3 5 8      卖半价
    MarkData _data;
    int curSaleCount;
    bool cansale;
    public override void init()
    {
        base.init();

        refreshItems();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        saleBtn.onClick.AddListener(clickSale);
        bussman.onClick.AddListener(clickinteract);
    }
    public override void reshow()
    {
        base.reshow();
        refreshItems();
    }
    void refreshItems()
    {
        curSaleCount = 0;
        cansale = false;
        _data = PlayerManager.Instance.GetMarkData();
        foreach (var i in saleItems)
            i.gameObject.SetActive(false);
        noneObj.SetActive(true);
        bussman.gameObject.SetActive(true);
        RunSingel.Instance.getBeiJingTime(result =>
        {
            //时间内
            if(result.Hour>=ConfigConst.markOpenTime && result.Hour <= ConfigConst.markEndTime)
            {
                saleBtn.gameObject.SetActive(true);
                if (_data.saleID.Length != 0)
                {
                    noneObj.SetActive(false);
                    //计算总数
                    for (int i = 0; i < _data.saleID.Length; i++)
                        curSaleCount += saleItems[i].getCount();

                    for (int i = 0; i < _data.saleID.Length; i++)
                    {
                        saleItems[i].setData(_data.saleID[i]);
                        saleItems[i].gameObject.SetActive(true);
                        curSaleCount += saleItems[i].getCount();
                    }
                }
                businessBag.text = curSaleCount + "/" + ConfigConst.markMaxCount;
                //数量
                if ((curSaleCount + _data.saledcount) >= ConfigConst.markMaxCount)
                {
                    saleBtn.GetComponent<Image>().color = Color.gray;
                    cansale = false;
                    businessBag.color = Color.red;
                }
                else
                {
                    saleBtn.GetComponent<Image>().color = Color.white;
                    cansale = true;
                    businessBag.color = Color.white;
                }
            }
            else
            {
                noneObj.SetActive(false);
                bussman.gameObject.SetActive(false);
                saleBtn.gameObject.SetActive(false);
            }
        });
    }
    void clickSale()
    {
        if (!cansale)
        {
            PanelManager.Instance.showTips3("商人的马车已经装满啦");
            return;
        }
        PanelManager.Instance.OpenPanel(E_UIPrefab.MarkSalePanel, new object[] { ConfigConst.markMaxCount - curSaleCount });
    }
    void clickinteract()
    {

    }
}

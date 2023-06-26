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
    public GameObject haventArriveObj;
    public Text businessBag;    //商人容量
    public Button bussman;
    public Image bussmancut;
    public Button back;
    //商人规则：每日10-17出现，交互随机对话，仓库容量总80，售卖物品容量30，槽位上限6，
    //售卖规则      1 2 3 5 8      卖半价
    MarkData _data;
    int curSaleCount;
    bool cansale;
    bool isarrive;
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
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
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
        isarrive = false;
        _data = PlayerManager.Instance.GetMarkData();
        foreach (var i in saleItems)
            i.gameObject.SetActive(false);
        noneObj.SetActive(true);
        haventArriveObj.SetActive(false);
        bussman.gameObject.SetActive(true);
        bussmancut.gameObject.SetActive(false);
        businessBag.text = "";
        saleBtn.GetComponent<Image>().color = Color.gray;
        RunSingel.Instance.getBeiJingTime(result =>
        {
            if(string.IsNullOrEmpty(_data.savetime) || (!string.IsNullOrEmpty(_data.savetime) && DateTime.Parse(_data.savetime) < result))
            {
                //新的物品
                PlayerManager.Instance.refreshNewMark();
            }
            Debug.Log(result.Hour);
            //时间内
            if(result.Hour>=ConfigConst.markOpenTime && result.Hour < ConfigConst.markEndTime)
            {
                isarrive = true;
                saleBtn.gameObject.SetActive(true);
                if (_data.saleID.Count != 0)
                {
                    noneObj.SetActive(false);
                    for (int i = 0; i < _data.saleID.Count; i++)
                    {
                        saleItems[i].setData(_data.saleID[i]);
                        saleItems[i].showpanel = showSalePanel;
                        saleItems[i].gameObject.SetActive(true);
                        curSaleCount += saleItems[i].getCount();
                    }
                }
                businessBag.text = curSaleCount + "/" + ConfigConst.markMaxCount;
                //数量
                if ((curSaleCount + _data.saledcount) >= ConfigConst.markMaxCount)
                {
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
                bussmancut.gameObject.SetActive(true);
                saleBtn.gameObject.SetActive(false);
                haventArriveObj.SetActive(true);
            }
        });
    }
    void showSalePanel(ItemData item)
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.MarkSalePanel, new object[] { ConfigConst.markMaxCount - curSaleCount , item });
    }
    void clickSale()
    {
        if (!isarrive)
        {
            PanelManager.Instance.showTips3("商人还未抵达");
            return;
        }
        if (!cansale)
        {
            PanelManager.Instance.showTips3("商人的马车已经装满啦");
            return;
        }
        showSalePanel(null);
    }
    void clickinteract()
    {

    }
}

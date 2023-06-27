using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class MarkSalePanel : PanelTopBase
{
    public GameObject messageBar;
    public Image _icon;
    public Text _sname;
    public Text _describe;
    public Text _onevalue;
    public Text _saleCount;
    public Button addbtn;
    public Button minusbtn;
    public Button allbtn;
    public Text _valueContext;  //价值
    public Text _residue;   //剩余空间
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Button confirm;
    public Button back;

    int residue;    //剩余空间
    t_items curitem;
    int curmax;
    int curnum;
    bool isinLimit; //add按钮限制
    public override void init()
    {
        base.init();
        residue = (int)message[0];
        scroll.initConfig(80, 80, clone);
        resetPanel();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        addbtn.onClick.AddListener(clickAdd);
        minusbtn.onClick.AddListener(clickMinus);
        allbtn.onClick.AddListener(clickAll);
        confirm.onClick.AddListener(clickExchange);
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }
    void resetPanel()
    {
        curmax = 0;
        curnum = 0;
        curitem = null;
        isinLimit = false;
        messageBar.SetActive(false);
        _valueContext.text = "0";
        _residue.text = residue.ToString();
        StartCoroutine(refreshScroll());
    }
    IEnumerator refreshScroll()
    {
        scroll.recycleAll();
        var datas = PlayerManager.Instance.playerItemDic;
        foreach(var i in datas)
        {
            var script = scroll.addItemDefault().GetComponent<BagItem>();
            script.setData(i.Key, i.Value);
            script.initAction(onClickOneItem);
        }
        scroll.reCalculateHeigh();
        yield return null;
    }
    void onClickOneItem(int id)
    {
        if (curitem!=null && curitem.id == id) return;
        //if 成功
        curnum = 0;
        curitem = Config_t_items.getOne(id);
        curmax = PlayerManager.Instance.playerItemDic[id];
        _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(ConfigConst.currencyID).iconName);
        _sname.text = curitem.sname;
        _describe.text = curitem.describe;
        _onevalue.text = curmax.ToString();
        _saleCount.text = curnum.ToString();
        if (residue == 0) setBtnState(true, false);
        setBtnState(false, false);
        messageBar.SetActive(true);
    }
    void clickAdd()
    {
        if (curnum == 0)
            setBtnState(false, true);
        curnum++;
        if (curnum >= curmax)
        {
            curnum = curmax;
            setBtnState(true, false);
            isinLimit = true;
        }
        if (curnum >= residue)
        {
            curnum = residue;
            setBtnState(true, false);
            isinLimit = true;
        }
        _saleCount.text = curnum.ToString();
        _valueContext.text = (curnum * curitem.pay).ToString();
    }
    void clickMinus()
    {
        curnum--;
        if (isinLimit)
            setBtnState(true, true);
        if (curnum <=0)
        {
            curnum = 0;
            setBtnState(false, false);
        }
        _saleCount.text = curnum.ToString();
        _valueContext.text = (curnum * curitem.pay).ToString();
    }
    void clickAll()
    {
        int num = Mathf.Min(curmax, residue);
        if (curnum == num) return;
        curnum = num;
        setBtnState(true, false);
        isinLimit = true;
        if (curnum > 0)
            setBtnState(false, true);
        _saleCount.text = curnum.ToString();
        _valueContext.text = (curnum * curitem.pay).ToString();
    }
    void setBtnState(bool isadd, bool isenable)
    {
        if (isadd)
        {
            addbtn.enabled = isenable;
            addbtn.GetComponent<Image>().color = isenable ? Color.white : Color.gray;
        }
        else
        {
            minusbtn.enabled = isenable;
            minusbtn.GetComponent<Image>().color = isenable ? Color.white : Color.gray;
        }
    }
    void clickExchange()
    {
        if(curitem.id== ConfigConst.currencyID)
        {
            PlayerManager.Instance.addItems(curitem.id, -curnum);
            PlayerManager.Instance.addSpecialMarkNum(curnum);
            PanelManager.Instance.showTips3("交易成功，失去银币X" + curnum);
        }
        else
        {
            residue -= curnum;
            PlayerManager.Instance.onMarkSale(curnum);
            PlayerManager.Instance.addItemsNosave(ConfigConst.currencyID, curnum * curitem.pay);
            PlayerManager.Instance.addItems(curitem.id, -curnum);
            PanelManager.Instance.showTips4(new List<ItemData>() { new ItemData(ConfigConst.currencyID, curnum * curitem.pay) });
        }
        resetPanel();
    }
}

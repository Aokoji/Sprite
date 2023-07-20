using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WareHousePanel : PanelTopBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;
    public UITool_ScrollView cardScroll;

    public GameObject itembar;
    public GameObject cardbar;

    public GameObject messageBar;
    public Image _icon;
    public Text _sname;
    public Text _describe;
    public Text _saleCount;
    public Text _haveCount;
    public Button _takeBtn;
    public Button _back;

    public Button check1;
    public Button check2;

    int curshowID;
    t_items curshowItem;
    int curcheck;
    string CARDPATH = "ui/battle/card/";
    public override void init()
    {
        base.init();
        setCheckBar(1);
        scroll.initConfig(80, 80, clone);
        var prefab = PanelManager.Instance.LoadUI(E_UIPrefab.cardShow, CARDPATH);
        cardScroll.initConfig(150, 200, prefab.gameObject);
        StartCoroutine(refreshScroll());
        StartCoroutine(refreshCardScroll());
    }
    public override void registerEvent()
    {
        base.registerEvent();
        _takeBtn.onClick.AddListener(useItem);
        _back.onClick.AddListener(PanelManager.Instance.DisposePanel);
        check1.onClick.AddListener(() => { setCheckBar(1); });
        check2.onClick.AddListener(() => { setCheckBar(2); });
    }
    IEnumerator refreshScroll()
    {
        //item scroll
        curshowID = 0;
        messageBar.SetActive(false);
        scroll.recycleAll();
        var data = PlayerManager.Instance.playerItemDic;
        foreach(var i in data)
        {
            var script = scroll.addItemDefault().GetComponent<BagItem>();
            script.setData(i.Key, i.Value);
            script.initAction(onclickOneItem);
        }
        yield return null;
    }
    IEnumerator refreshCardScroll()
    {
        cardScroll.recycleAll();
        var normallist = TableManager.Instance.basicList;
        foreach (var item in normallist)
        {
            var config = Config_t_DataCard.getOne(item);
            var card = cardScroll.addItemDefault().GetComponent<CardSetEntity>();
            card.initData(item, 2, chooseCard);
        }
        var data = PlayerManager.Instance.playerMakenDic;
        foreach(var item in data)
        {
            var config = Config_t_DataCard.getOne(item.Key);
            var card = cardScroll.addItemDefault().GetComponent<CardSetEntity>();
            card.initData(item.Key, 2, chooseCard);
        }
        cardScroll.reCalculateHeigh();
        yield return null;
    }
    void chooseCard(CardSetEntity card)
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.CardMessageBar, new object[] { card._data.id });
    }
    //选中item物品
    void onclickOneItem(int id)
    {
        if (curshowID == id) return;
        curshowID = id;
        curshowItem = Config_t_items.getOne(id);
        _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), curshowItem.iconName);
        _sname.text = curshowItem.sname;
        _describe.text = curshowItem.describe;
        _haveCount.text = PlayerManager.Instance.playerItemDic[id].ToString();
        if (curshowItem.type == ItemsType.consum)
        {
            _takeBtn.gameObject.SetActive(true);
        }
        else
        {
            _takeBtn.gameObject.SetActive(false);
        }
        _saleCount.text = "价值：" + curshowItem.pay;
        messageBar.SetActive(true);
    }
    void setCheckBar(int barnum)
    {
        if (curcheck == barnum) return;
        curcheck = barnum;
        itembar.SetActive(barnum==1);
        cardbar.SetActive(barnum==2);
        check1.GetComponent<Image>().color = barnum == 1 ? Color.white : Color.gray;
        check2.GetComponent<Image>().color = barnum == 2 ? Color.white : Color.gray;
    }
    void useItem()
    {
        //curshowID +++
        switch (curshowItem.type2)
        {
            case ItemType2.plan:
                //设计图
                if (PlayerManager.Instance.LearnPlan(curshowItem.connect))
                {
                    PlayerManager.Instance.addItems(curshowID, -1);
                    PanelManager.Instance.showTips3("使用成功");
                }
                else
                    PanelManager.Instance.showTips3("使用失败，已学会该配方");
                break;
            case ItemType2.stone:
                //消耗品
                break;
            default:
                PanelManager.Instance.showTips3("使用失败");
                return;
        }
        StartCoroutine(refreshScroll());
    }
}

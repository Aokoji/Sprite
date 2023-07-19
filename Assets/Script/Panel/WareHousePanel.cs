using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WareHousePanel : PanelTopBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;

    public GameObject messageBar;
    public Image _icon;
    public Text _sname;
    public Text _describe;
    public Text _saleCount;
    public Text _haveCount;
    public Button _takeBtn;
    public Button _back;

    int curshowID;
    t_items curshowItem;
    public override void init()
    {
        base.init();
        scroll.initConfig(80, 80, clone);
        StartCoroutine(refreshScroll());
    }
    public override void registerEvent()
    {
        base.registerEvent();
        _takeBtn.onClick.AddListener(useItem);
        _back.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }
    IEnumerator refreshScroll()
    {
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

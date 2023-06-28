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
    public override void init()
    {
        base.init();
        scroll.initConfig(80, 80, clone);
        curshowID = 0;
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
        var data = Config_t_items.getOne(id);
        _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), data.iconName);
        _sname.text = data.sname;
        _describe.text = data.describe;
        _haveCount.text = PlayerManager.Instance.playerItemDic[id].ToString();
        if (data.type == (int)ItemsType.consum)
        {
            _takeBtn.gameObject.SetActive(true);
        }
        else
        {
            _takeBtn.gameObject.SetActive(false);
        }
        _saleCount.text = "价值：" + data.pay;
        messageBar.SetActive(true);
    }
    void useItem()
    {
        //curshowID +++
    }
}

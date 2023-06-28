using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopPanel : PanelBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;

    public Button typeConsum;   //消耗品
    public Button typeCard;     //卡
    public Button typeMagic;        //魔法书

    public GameObject panelMessage;     //详情
    public Image _icon;
    public Text _sname;
    public Text _describe;
    public Text _describeExtra;
    public Text _have;
    public Image[] icons;
    public Text[] contexts;
    public Button _make;
    public Button _Allmake;

    int curPanel;
    int curshowID;
    int finalID;
    int canmakeMax;
    t_items cardData;
    public override void init()
    {
        base.init();
        curPanel = 0;
        curshowID = 0;
        finalID = 0;
        canmakeMax = 0;
        cardData = Config_t_items.getOne(ConfigConst.cardGeneralID);
        _describeExtra.text = cardData.describe;
        scroll.initConfig(80, 80, clone);
        StartCoroutine(refreshScroll());
    }
    public override void registerEvent()
    {
        base.registerEvent();
        _make.onClick.AddListener(clickMake);
        _Allmake.onClick.AddListener(clickAllMake);
    }

    IEnumerator refreshScroll()
    {
        panelMessage.SetActive(false);
        scroll.recycleAll();
        var data = PlayerManager.Instance.playerItemDic;
        foreach (var i in data)
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
        var plandata= Config_t_Plan.getOne(id);
        finalID = plandata.finishID;
        if (curPanel == 1)
        {
            //卡
            var data = Config_t_DataCard.getOne(finalID);
            _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), cardData.iconName);
            _sname.text = data.sname;
            _describe.text = data.sDescribe;
            _have.text = "拥有：" + PlayerManager.Instance.getOneCardNum(id);
            _describeExtra.gameObject.SetActive(true);
        }
        else
        {
            var data = Config_t_items.getOne(finalID);
            _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), data.iconName);
            _sname.text = data.sname;
            _describe.text = data.describe;
            _have.text = "拥有："+ PlayerManager.Instance.playerItemDic[id];
            _describeExtra.gameObject.SetActive(false);
        }
        string[] strid = plandata.needid.Split('|');
        string[] strnum = plandata.needcount.Split('|');
        for(int i = 0; i < icons.Length; i++)
        {
            if (i < strid.Length)
            {
                int thisid = int.Parse(strid[i]);
                int neednum = int.Parse(strnum[i]);
                var needitem = Config_t_items.getOne(thisid);
                icons[i].sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), needitem.iconName);
                int have = PlayerManager.Instance.getItem(thisid);
                contexts[i].text = have + "/" + strnum[i];
                //颜色
                if (have < neednum)
                {
                    contexts[i].color = Color.red;
                    canmakeMax = 0;
                }
                else
                {
                    contexts[i].color = Color.white;
                    canmakeMax = Mathf.Min(canmakeMax, have / neednum);
                }
            }
        }
        panelMessage.SetActive(true);
    }
    void clickMake()
    {
        if (canmakeMax == 0)
        {
            PanelManager.Instance.showTips1("材料不足");
            return;
        }
        //PanelManager.Instance.showTips2("确认制作‘"++"’")
    }
    void clickAllMake()
    {
        if (canmakeMax == 0)
        {
            PanelManager.Instance.showTips1("材料不足");
            return;
        }
    }
}

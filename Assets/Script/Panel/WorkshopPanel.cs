using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopPanel : PanelTopBase
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
    public Button back;

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
        scroll.initConfig(260, 50, clone);
        StartCoroutine(refreshScroll());
    }
    public override void registerEvent()
    {
        base.registerEvent();
        _make.onClick.AddListener(clickMake);
        _Allmake.onClick.AddListener(clickAllMake);
        typeConsum.onClick.AddListener(clickConsum);
        typeCard.onClick.AddListener(clickCard);
        typeMagic.onClick.AddListener(clickMagic);
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }

    IEnumerator refreshScroll()
    {
        panelMessage.SetActive(false);
        scroll.recycleAll();
        var data = PlayerManager.Instance.getLearnList();
        foreach (var i in data)
        {
            var plan= Config_t_Plan.getOne(i);
            if (curPanel != plan.finishType) continue;
            var script = scroll.addItemDefault();
            script.GetComponentInChildren<Text>().text = plan.remark;
            int index = i;
            script.GetComponent<Button>().onClick.AddListener(()=> { onclickOneItem(index); });
        }
        yield return null;
    }
    t_items curitem;
    t_DataCard curcard;
    void onclickOneItem(int id)
    {
        if (curshowID == id) return;
        curshowID = id;
        var plandata= Config_t_Plan.getOne(id);
        finalID = plandata.finishID;
        canmakeMax = 100;
        if (curPanel == 1)
        {
            //卡
            curcard = Config_t_DataCard.getOne(finalID);
            _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), cardData.iconName);
            _sname.text = curcard.sname;
            _describe.text = curcard.sDescribe;
            _have.text = "拥有：" + PlayerManager.Instance.getOneCardNum(id);
            _describeExtra.gameObject.SetActive(true);
        }
        else
        {
            curitem = Config_t_items.getOne(finalID);
            _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), curitem.iconName);
            _sname.text = curitem.sname;
            _describe.text = curitem.describe;
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
                icons[i].gameObject.SetActive(true);
                contexts[i].gameObject.SetActive(true);
            }
            else
            {
                icons[i].gameObject.SetActive(false);
                contexts[i].gameObject.SetActive(false);
            }
        }
        if (canmakeMax == 0)
        {
            _make.GetComponent<Image>().color = Color.gray;
            _Allmake.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            _make.GetComponent<Image>().color = Color.white;
            _Allmake.GetComponent<Image>().color = Color.white;
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
        string sname;
        if (curPanel == 1)
            sname = curcard.sname;
        else
            sname = curitem.sname;
        PanelManager.Instance.showTips2("确认制作‘" + sname + "’吗？", () =>
        {
            PlayerManager.Instance.makenItem(curshowID, 1, makeItem);
        });
    }
    void makeItem()
    {
        int id = curshowID;
        curshowID = 0;
        onclickOneItem(id);
        //短动画
    }
    void clickAllMake()
    {
        if (canmakeMax == 0)
        {
            PanelManager.Instance.showTips1("材料不足");
            return;
        }
        string sname;
        if (curPanel == 1)
            sname = curcard.sname;
        else
            sname = curitem.sname;
        PanelManager.Instance.showTips2("确认制作‘" + sname + "’×" + canmakeMax + "吗？", () =>
        {
            PlayerManager.Instance.makenItem(curshowID, canmakeMax, makeItem);
        });
    }
    void clickConsum()
    {
        if (curPanel == 0) return;
        curPanel = 0;
        StartCoroutine(refreshScroll());
    }
    void clickCard()
    {
        if (curPanel == 1) return;
        curPanel = 1;
        StartCoroutine(refreshScroll());
    }
    void clickMagic()
    {
        if (curPanel == 2) return;
        curPanel = 2;
        StartCoroutine(refreshScroll());
    }
}

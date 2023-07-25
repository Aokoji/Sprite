using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorPanel : PanelBase
{
    //关卡每天随机一种天气或地图
    public Button enter1;
    public Button enter2;
    public ExplorQuestBar[] offers; //四个告示      悬赏/交易/

    public Button back;
    public Button dayReward;
    public Button daytipbar;
    public Image dayicon;
    public Text daycontext;

    public Image _spicon;
    public Text _sname;
    public Text _curhp;
    public Text _curphy;
    public Button setCardBtn;
    public Text _curbag;
    public Button setBagBtn;

    ExplorData _data;
    public override void init()
    {
        base.init();
        _data = PlayerManager.Instance.getExplorData();
        refreshMap();
    }
    public override void registerEvent()
    {
        base.registerEvent();  
        enter1.onClick.AddListener(clickEnter1);
        enter2.onClick.AddListener(clickEnter2);
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
        dayReward.onClick.AddListener(showDayTips);
        setCardBtn.onClick.AddListener(showCardset);
        setBagBtn.onClick.AddListener(showBagset);
        daytipbar.onClick.AddListener(closeDayTips);
        _spicon.GetComponent<Button>().onClick.AddListener(clickSpChoose);
    }
    public override void reshow()
    {
        base.reshow();
        refreshMap();
    }
    void refreshMap()
    {
        daytipbar.gameObject.SetActive(false);
        RunSingel.Instance.getBeiJingTime(result =>
        {
            if (string.IsNullOrEmpty(_data.savetime) || (!string.IsNullOrEmpty(_data.savetime) && DateTime.Parse(_data.savetime) < result))
            {
                //新的一天
                PlayerManager.Instance.refreshNewDayExplor();
            }
            //文案做在图上
            enter1.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.changeSprite.ToString(), Config_t_ExplorMap.getOne(_data.mapType).imageName);

            //offer
            for(int i = 0; i < offers.Length; i++)
            {
                if (i < _data.offer.Count)
                {
                    offers[i].setData(_data.offer[i]);
                    offers[i].gameObject.SetActive(true);
                }
                else
                    offers[i].gameObject.SetActive(false);
            }
            if (PlayerManager.Instance.getExplorData().dayboss > 0)
                dayReward.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), "");
            else
                dayReward.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), "");
        });
        //当前状态（不需要时间
        var cursp = PlayerManager.Instance.getcursprite();
        _spicon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), cursp.icon);
        _sname.text = cursp.sname;
        _curhp.text= cursp.hp_cur + "/" + cursp.hp_max;
        _curphy.text= cursp.phy_cur + "/" + cursp.phy_max;
        _curbag.text = PlayerManager.Instance.getExplorData().explorBag.Count+"/5";  //负重上限暂时为5
    }

    void clickEnter1()
    {
        //进入森林
        PanelManager.Instance.ChangeScenePanel(E_UIPrefab.ExplorMovingPanel, new object[] { true });
    }
    void clickEnter2()
    {
        //森林小径
        PanelManager.Instance.ChangeScenePanel(E_UIPrefab.ExplorMovingPanel, new object[] { false });
    }
    void clickSpChoose()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.SpriteCheckPanel, new object[] { spriteChooseType.changeCur });
    }
    void showDayTips()
    {
        var conf = Config_t_items.getOne(_data.dayboss);
        dayicon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), conf.iconName);
        daycontext.text = conf.sname;
        daytipbar.gameObject.SetActive(true);
    }
    void closeDayTips()
    {
        daytipbar.gameObject.SetActive(false);
    }
    void showCardset()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.CardsetPanel);
    } 
    void showBagset()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.ExplorBagSetPanel);
    }
    //文案    9
    //今天的迷雾格外浓厚，实在是无法深入呢。
    //迷雾较为浓厚，在周边小心探索为好。 4关
    //森林仍然被迷雾笼罩，准备好提灯小心探索。  6关
    //今日下雨，迷雾稍有消散，可以采集一些资源。     8关
    //迷雾较稀，森林偶有咆哮声，似乎有野兽出没。 9关
    //迷雾稀薄，适合适当的深入，但要小心行事。      12关
    //迷雾暂时消散，凶猛的魔物也开始四处活动了，但同时也是寻找宝物的好时机！15关
    //森林中传出猛烈的魔物气息，似乎有庞然大物经过。   5关
    //乌云笼罩，似乎有领主苏醒了。    20关
}

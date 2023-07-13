using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class ExplorGatherMessPanel : PanelTopBase
{
    public Text title;
    public Image gatherIcon;
    public Text gatherName;
    public Text context;
    public Button giveUpBtn;   //放弃
    public Button takeBtn;  //
    public Text takeContext;

    ExplorMovingPanel.rankReward _data;
    public override void init()
    {
        base.init();
        _data = message[0] as ExplorMovingPanel.rankReward;
        checkType();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        giveUpBtn.onClick.AddListener(clickGiveUpBtn);
        takeBtn.onClick.AddListener(clickTakeBtn);
    }
    void checkType()
    {
        var dat = Config_t_items.getOne(_data.sbox.id);
        if (_data.stype == explorIcon.gather)
        {
            title.text = "资源采集";
            gatherIcon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), dat.iconName);
            gatherName.text = dat.sname + "×" + _data.sbox.num;
            context.text = "发现可采集资源";
            takeContext.text = "采  集";
        }
        else if (_data.stype == explorIcon.box)
        {
            title.text = "事件详情";
            gatherIcon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), "losePackage");
            gatherName.text = "遗落的行李";
            context.text = "发现了遗落在森林的旅人行李，是否要搜索一番？";
            takeContext.text = "搜  索";
        }
    }

    void clickTakeBtn()
    {
        if (PlayerManager.Instance.getcursprite().phy_cur < 1)
        {
            PanelManager.Instance.showTips3("妖精体力不足！");
            return;
        }
        PlayerManager.Instance.getcursprite().phy_cur -= 1;
        var reward = new List<ItemData>() { _data.sbox };
        PlayerManager.Instance.addItems(reward);
        PanelManager.Instance.showTips3("妖精体力-1");
        PanelManager.Instance.showTips5("获得道具", reward, () =>
        {
            EventAction.Instance.TriggerAction(eventType.explorGoNextRank_B, true);
            PanelManager.Instance.DisposePanel();
        });
    }
    void clickGiveUpBtn()
    {
        EventAction.Instance.TriggerAction(eventType.explorGoNextRank_B, true);
        PanelManager.Instance.DisposePanel();
    }
}

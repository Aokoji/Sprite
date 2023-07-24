using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorOfferMorePanel : PanelTopBase
{
    public Text title;
    public Text describe;
    public Text frontFInish;    //finish的描述
    public Text finishcount;        //or  have
    public Text reward;
    public Image rewardimg;
    public Button back;
    public Button goBattle;
    public Button finishBtn;

    OfferData _data;
    t_Offer config;
    bool isfinish;
    offerType configtype;

    public override void init()
    {
        base.init();
        _data = message[0] as OfferData;
        config = Config_t_Offer.getOne(_data.id);
        title.text = config.title;
        describe.text = config.describe;
        configtype = (offerType)config.stype;

        //检查complete
        goBattle.gameObject.SetActive(false);
        if (configtype == offerType.battle)
        {
            frontFInish.text = "击败数量：";
            if (_data.finishCount >= config.finishNum)
                isfinish = true;
            else
                goBattle.gameObject.SetActive(true);
            finishcount.text = _data.finishCount + "/" + config.finishNum;
        }
        else if(configtype==offerType.count)
        {
            frontFInish.text = "击败数量：";
            if (_data.finishCount >= config.finishNum)
                isfinish = true;
            finishcount.text = _data.finishCount + "/" + config.finishNum;
        }
        else
        {
            frontFInish.text = "收集进度：";
            int have = PlayerManager.Instance.getItem(config.targetID);
            if (have >= config.finishNum)
                isfinish = true;
            finishcount.text = have + "/" + config.finishNum;
        }
        finishBtn.gameObject.SetActive(isfinish);
    }
    public override void registerEvent()
    {
        base.registerEvent();
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
        finishBtn.onClick.AddListener(completeaction);
        goBattle.onClick.AddListener(goBattleaction);
    }

    void completeaction()
    {
        ItemData result;
        if (configtype == offerType.change)
        {
            PlayerManager.Instance.addItems(config.changeID, config.changeNum);
            result = new ItemData(config.changeID, config.changeNum);
        }
        else
        {
            PlayerManager.Instance.addItems(ConfigConst.currencyID, config.reward);
            result = new ItemData(ConfigConst.currencyID, config.reward);
        }
        PanelManager.Instance.showTips4(new List<ItemData>() { result });
        PanelManager.Instance.DisposePanel();
    }
    void goBattleaction()
    {
        ExplorMovingPanel.rankReward config1 = new ExplorMovingPanel.rankReward();
        config1.enemyID = config.targetID;
        config1.stype = explorIcon.battle;
        config1.isboss = false;
        config1.isexploring = false;
        PanelManager.Instance.OpenPanel(E_UIPrefab.ExplorBattleMessPanel, new object[] { config1 });
    }
}

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
    public Button back;
    public Button goBattle;
    public Text goBtnText;

    OfferData _data;
    bool isfinish;
    offerType configtype;

    public override void init()
    {
        base.init();
        _data = message[0] as OfferData;
        var config = Config_t_Offer.getOne(_data.id);
        title.text = config.title;
        describe.text = config.describe;
        configtype = (offerType)config.stype;
        if (configtype == offerType.change)
            frontFInish.text = "收集进度：";
        else
            frontFInish.text = "击败数量：";

        //检查complete

        if(configtype == offerType.battle)
        {
            goBattle.gameObject.SetActive(true);
            goBtnText.text = "";
        }
        else
        {
            goBattle.gameObject.SetActive(false);
        }
    }
    public override void registerEvent()
    {
        base.registerEvent();
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
        goBattle.onClick.AddListener(completeaction);
    }

    void completeaction()
    {

    }
}

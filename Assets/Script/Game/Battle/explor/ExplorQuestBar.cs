using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorQuestBar : UIBase
{
    public Text title;
    public Text reward;
    public Button showMoreBtn;

    OfferData _data;
    private void Start()
    {
        showMoreBtn.onClick.AddListener(onclickShowMore);
    }

    public void setData(OfferData data)
    {
        _data = data;
        var ddd = Config_t_Offer.getOne(data.id);
        title.text = ddd.title;
        reward.text = "×" + ddd.reward;

    }
    void onclickShowMore()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.ExplorOfferMorePanel, new object[] { _data });
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelSpriteMessageBar : UIBase
{
    public Image icon;
    public Text spname;
    public Text phycontext;
    public Button gotravelBtn;
    public Text travelBtnContext;
    SpriteData _data;
    public void initAction(Action<int> gotravel)
    {
        gotravelBtn.onClick.AddListener(() =>{ gotravel(_data.id); });
    }
    public void setData(SpriteData data,E_UIPrefab pref=E_UIPrefab.none)
    {
        _data = data;
        spname.text = data.sname;
        phycontext.text = data.phy_cur + "/" + data.phy_max;
        icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), data.icon);
        switch (pref)
        {
            case E_UIPrefab.TravelSpriteMessagePanel:
                if (data.istraveling||data.isworking)
                {
                    gotravelBtn.GetComponent<Image>().color = Color.gray;
                    if(data.istraveling) { travelBtnContext.text = "旅行中.."; }
                    else travelBtnContext.text = "工作中..";
                }
                else
                {
                    gotravelBtn.GetComponent<Image>().color = Color.white;
                    travelBtnContext.text = "旅行";
                }
                break;
            case E_UIPrefab.MillSpriteWorkPanel:
                if (data.phy_cur == 0)
                    gotravelBtn.GetComponent<Image>().color = Color.gray;
                else
                    gotravelBtn.GetComponent<Image>().color = Color.white;
                travelBtnContext.text = "委派工作";
                break;
        }

    }
}

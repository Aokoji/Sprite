using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCheckBar : UIBase
{
    public Image icon;
    public Text spname;
    public Text healcontext;
    public Text phycontext;
    public Button gotravelBtn;
    public Text travelBtnContext;
    SpriteData _data;
    bool allow;
    public void initAction(Action<int> callback)
    {
        gotravelBtn.onClick.AddListener(() =>
        {
            if(allow)
                callback?.Invoke(_data.id);
            else
                PanelManager.Instance.showTips3("该妖精不可选择");
        });
    }
    public void setData(SpriteData data, spriteChooseType stype)
    {
        _data = data;
        spname.text = data.sname;
        phycontext.text = data.phy_cur + "/" + data.phy_max;
        healcontext.text = data.hp_cur + "/" + data.hp_max;
        icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), data.icon);
        travelBtnContext.text = "选择";
        gotravelBtn.GetComponent<Image>().color = Color.white;
        allow = true;
        switch (stype)
        {
            case spriteChooseType.changeCur:
                if (data.istraveling)
                {
                    gotravelBtn.GetComponent<Image>().color = Color.gray;
                    travelBtnContext.text = "旅行中..";
                    allow = false;
                }
                if(data.isworking)
                {
                    gotravelBtn.GetComponent<Image>().color = Color.gray;
                    travelBtnContext.text = "工作中..";
                    allow = false;
                }
                break;
            case spriteChooseType.useSth:
                break;
            default:
                break;
        }

    }
}

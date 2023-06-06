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
    SpriteData _data;
    public void initAction(Action<int> gotravel)
    {
        gotravelBtn.onClick.AddListener(() =>{ gotravel(_data.id); });
    }
    public void setData(SpriteData data)
    {
        _data = data;
        spname.text = data.sname;
        phycontext.text = data.phy_cur + "/" + data.phy_max;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelSpriteMessagePanel : PanelTopBase
{
    public Button backBtn;
    public UITool_ScrollView scroll;
    public TravelSpriteMessageBar clone;
    public override void init()
    {
        base.init();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        backBtn.onClick.AddListener(clickClose);
    }

    void refreshSprites()
    {

    }

    void clickClose()
    {
        PanelManager.Instance.DisposePanel();
    }
}

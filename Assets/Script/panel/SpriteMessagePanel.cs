using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMessagePanel : PanelTopBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Image _icon;
    public Text _sname;
    public Text _health;
    public Text _phy;
    public Text _powerPoint;

    public Text _messFire;
    public Text _messWater;
    public Text _messForest;
    public Text _messThunder;
    public Text _messArcane;
    public Text _messGolden;
    public override void init()
    {
        base.init();
    }
    public override void registerEvent()
    {
        base.registerEvent();
    }
}

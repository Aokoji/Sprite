using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpriteMessagePanel : PanelTopBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;
    public GameObject parentBar;
    public Image _icon;
    public Image _whole;
    public Text _sname;
    public Text _health;
    public Text _phy;
    public Text _powerPoint;

    public Text _messFire;
    public Text _messWater;
    public Text _messForest;
    public Text _messThunder;
    public Text _messArcane;
    public Text _messGoden;
    public Image _imgFire;
    public Image _imgWater;
    public Image _imgForest;
    public Image _imgThunder;
    public Image _imgArcane;
    public Image _imgGoden;

    int curid;
    public override void init()
    {
        base.init();
        scroll.initConfig(250, 74, clone);
        StartCoroutine(initScroll());
    }
    public override void registerEvent()
    {
        base.registerEvent();
    }

    IEnumerator initScroll()
    {
        scroll.recycleAll();
        var data = PlayerManager.Instance.spriteList;
        curid = data.First().Key;
        setData();
        foreach (var i in data)
        {
            var obj = scroll.addItemDefault();
            obj.GetComponentInChildren<Text>().text = i.Value.sname;
            int index = i.Key;
            obj.GetComponent<Button>().onClick.AddListener(()=> { clickTag(index); });
        }
        scroll.reCalculateHeigh();
        yield return null;
    }

    void clickTag(int id)
    {
        if (curid == id) return;
        curid = id;
        setData();
    }
    void setData()
    {
        var data = PlayerManager.Instance.spriteList[curid];
        _icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), data.icon);
        _whole.sprite = GetSprite(A_AtlasNames.wholeImg.ToString(), Config_t_ActorMessage.getOne(curid).wholeIcon);
        _sname.text = data.sname;
        _health.text = "生命值：" + data.hp_cur + "/" + data.hp_max;
        _phy.text = "体力值：" + data.phy_cur + "/" + data.phy_max;
        _powerPoint.text = "能量点数：" + data.spritePower;
        var ability = Config_t_TakeCardLevel.getOne(curid);
        _messFire.text = ability.fire.ToString();
        _messWater.text = ability.water.ToString();
        _messForest.text = ability.forest.ToString();
        _messThunder.text = ability.thunder.ToString();
        _messArcane.text = ability.arcane.ToString();
        _messGoden.text = ability.goden.ToString();
    }
}

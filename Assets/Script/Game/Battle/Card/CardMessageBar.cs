using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMessageBar : PanelTopBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Button bgclose;

    public GameObject explainbar;
    public Text sname;
    public Text descirbe;
    public Text cost;
    public Image bg;
    public Text level;
    public Text cardtype;

    t_DataCard _data;
    public override void init()
    {
        base.init();
        _data = Config_t_DataCard.getOne(int.Parse(message[0].ToString()));
        setcard();
        scroll.initConfig(80, 80, clone);   //+++词条
        StartCoroutine(initscroll());
    }
    public override void registerEvent()
    {
        base.registerEvent();
        bgclose.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }
    void setcard()
    {
        sname.text = _data.sname.ToString();
        descirbe.text = _data.sDescribe.ToString();
        cost.text = _data.cost.ToString();
        bg.color = Color.white;
        bg.sprite = GetSprite(A_AtlasNames.atlasImg1.ToString(), "card" + (int)_data.limit);
        level.text = _data.level.ToString();
        cardtype.text = ConfigConst.getCardType(_data.type1);
    }
    IEnumerator initscroll()
    {
        scroll.recycleAll();
        string[] str = _data.sname.Split('|');
        if (str.Length == 1 && str[0].Equals("0"))
            explainbar.SetActive(false);
        else
        {
            foreach (var i in str)
            {
                var obj = scroll.addItemDefault();
                obj.GetComponentInChildren<Text>().text = ConfigConst.entryname(i);
            }
            explainbar.SetActive(true);
            scroll.reCalculateHeigh();
        }
        yield return null;
    }
}

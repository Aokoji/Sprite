using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPop5 : TipsBase
{
    public Text titleText;
    public GameObject cloneBar;
    public UITool_ScrollView scroll;
    public Button ok;
    private void Start()
    {
        ok.onClick.AddListener(clickok);
    }

    public override void init(string title, List<ItemData> whatthing, Action act)
    {
        allowClick = false;
        callback = act;
        titleText.text = title;
        cloneBar.SetActive(false);
        scroll.initConfig(100, 100, cloneBar);
        scroll.recycleAll();
        context.text = "";
        foreach (var i in whatthing)
        {
            var obj = scroll.addItemDefault();
            var dat = Config_t_items.getOne(i.id);
            obj.GetComponentInChildren<Text>().text = dat.sname + "×" + i.num;
            obj.GetComponentInChildren<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), dat.iconName);
        }
    }
    public override void setString2(string str)
    {
        context.text = str;
    }
    public override void play()
    {
        AnimationTool.playAnimation(gameObject, "showtip2", false, () => { allowClick = true; });
    }
    void clickok()
    {
        if (!allowClick) return;
        callback?.Invoke();
        callback = null;
        gameObject.SetActive(false);
    }
}

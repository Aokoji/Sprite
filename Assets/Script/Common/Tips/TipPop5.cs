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
    bool iscut;
    private void Start()
    {
        ok.onClick.AddListener(clickok);
    }

    public override void init(string title, List<ItemData> whatthing, Action act)
    {
        allowClick = false;
        iscut = false;
        if (callback != null)
            iscut = true;
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
    public override void init(object whatthing)
    {
        allowClick = false;
        iscut = false;
        if (callback != null)
            iscut = true;
        titleText.text = "获得物品";
        cloneBar.SetActive(false);
        scroll.initConfig(100, 100, cloneBar);
        scroll.recycleAll();
        context.text = "";
        ItemData itm = whatthing as ItemData;
        var obj = scroll.addItemDefault();
        var dat = Config_t_items.getOne(itm.id);
        obj.GetComponentInChildren<Text>().text = dat.sname + "×" + itm.num;
        obj.GetComponentInChildren<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), dat.iconName);
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
        gameObject.SetActive(false);
        callback?.Invoke();
        if(!iscut)
            callback = null;
        iscut = false;
    }
}

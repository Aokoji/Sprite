using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCheckPanel : PanelTopBase
{
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Button backBtn;

    spriteChooseType stype;
    public override void init()
    {
        base.init();
        stype = (spriteChooseType)(int)message[0];
        clone.SetActive(false);
        scroll.initConfig(350, 100, clone.gameObject);
        StartCoroutine(refreshScrollData());
    }
    public override void registerEvent()
    {
        base.registerEvent();
        backBtn.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }

    IEnumerator refreshScrollData()
    {
        scroll.recycleAll();
        foreach (var item in PlayerManager.Instance.spriteList)
        {
            var obj = scroll.addItemDefault().GetComponent<SpriteCheckBar>();
            obj.initAction(onChoose);
            obj.setData(item.Value, stype);
            obj.gameObject.SetActive(true);
        }
        scroll.reCalculateHeigh();
        yield return null;
    }

    void onChoose(int id)
    {
        if (stype == spriteChooseType.changeCur)
        {
            PlayerManager.Instance.changeSprite(id);
            PanelManager.Instance.showTips3("切换成功");
        }
        else if(stype == spriteChooseType.useSth)
        {
            int index= (int)message[1];
            bool success = PlayerManager.Instance.useProp(index, id);
            if(success)
                PanelManager.Instance.showTips3("使用成功");
            else
            {
                PanelManager.Instance.showTips3("使用失败，属性已满");
                return;
            }
            //StartCoroutine(refreshScrollData());
        }
        PanelManager.Instance.DisposePanel();
    }
}

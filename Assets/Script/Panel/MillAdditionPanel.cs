using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class MillAdditionPanel : PanelTopBase
{
    public Button addmater1;
    public Button addmater10;
    public Button reducemater1;
    public Button reducemater10;
    public Image icon;
    public Text contextcount;
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Button confirm;
    public Button back;
    int curid;
    int curcount;
    int materid;

    public override void init()
    {
        base.init();
        curid = (int)message[0];
        materid = (int)message[1];
        curcount = 0;
        if (curid == 0)
            //icon置空图
            icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), ConfigConst.NOIMG_ICON);
        else
            icon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(curid).iconName);
    }
    public override void registerEvent()
    {
        base.registerEvent();
        addmater1.onClick.AddListener(() => { addCount(1); });
        addmater10.onClick.AddListener(() => { addCount(10); });
        reducemater1.onClick.AddListener(() => { addCount(-1); });
        reducemater10.onClick.AddListener(() => { addCount(-10); });
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
        confirm.onClick.AddListener(clickConfirm);
        EventAction.Instance.AddEventGather<int>(eventType.millTimerMater_I, refreshCount);
    }
    public override void unregisterEvent()
    {
        base.unregisterEvent();
        EventAction.Instance.RemoveAction<int>(eventType.millTimerMater_I, refreshCount);
    }

    void refreshCount(int id)
    {
        if (id == curid)
        {
            curcount--;
            if (curcount <= 0)
                curcount = 0;
        }
    }
    #region click
    void addCount(int count)
    {
        //检测上限+++
        curcount += count;
        if (curcount <= 0)
            curcount = 0;
        if (PlayerManager.Instance.playerItemDic[curid] < curcount)
            curcount = PlayerManager.Instance.playerItemDic[curid];
        contextcount.text = curcount.ToString();
    }
    void clickConfirm()
    {
        if (curcount == 0)
        {
            PanelManager.Instance.showTips3("没有添加材料");
            return;
        }
        if (materid == 1)
            EventAction.Instance.TriggerAction(eventType.millAddMater1_II, curid, curcount);
        else
            EventAction.Instance.TriggerAction(eventType.millAddMater1_II, curid, curcount);
        PanelManager.Instance.DisposePanel();
    }
    #endregion
}

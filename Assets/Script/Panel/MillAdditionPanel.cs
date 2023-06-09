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
    public Button itemImg;
    public Text contextcount;
    public UITool_ScrollView scroll;
    public GameObject clone;
    public Button back;
    public Text gringding;  //研磨中字样
    public Button cancelGring;  //取消研磨

    int curid;
    int curcount;
    bool ismater2;
    int exid;
    int excount;

    public override void init()
    {
        base.init();
        ismater2 = (bool)message[0];
        curid = exid = (int)message[1];
        curcount = excount = (int)message[2];
        refreshMainmessage();

    }
    public override void registerEvent()
    {
        base.registerEvent();
        addmater1.onClick.AddListener(() => { addCount(1); });
        addmater10.onClick.AddListener(() => { addCount(10); });
        reducemater1.onClick.AddListener(() => { addCount(-1); });
        reducemater10.onClick.AddListener(() => { addCount(-10); });
        back.onClick.AddListener(PanelManager.Instance.DisposePanel);
        itemImg.onClick.AddListener(clickChooseItem);
        EventAction.Instance.AddEventGather<int>(eventType.millTimerMater_I, refreshCount);
    }
    public override void unregisterEvent()
    {
        base.unregisterEvent();
        EventAction.Instance.RemoveAction<int>(eventType.millTimerMater_I, refreshCount);
    }
    void refreshMainmessage()
    {
        if (curid == 0)
        {
            itemImg.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), ConfigConst.NOIMG_ICON);
            cancelGring.gameObject.SetActive(true);
            gringding.text = "研磨中...";
        }
        else
        {
            itemImg.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(curid).iconName);
            cancelGring.gameObject.SetActive(false);
            gringding.text = "待选择要研磨的材料";
        }
    }

    void refreshCount(int id)
    {
        if (id == exid)
        {
            excount--;
            if (excount <= 0)
                excount = 0;
            if (exid == curid)
            {

            }
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
        if (ismater2)
            EventAction.Instance.TriggerAction(eventType.millAddMater2_II, curid, curcount);
        else
            EventAction.Instance.TriggerAction(eventType.millAddMater1_II, curid, curcount);
        PanelManager.Instance.DisposePanel();
    }
    void clickChooseItem()
    {

    }
    #endregion
}

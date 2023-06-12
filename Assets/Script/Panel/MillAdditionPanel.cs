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
    public Text addingContext;
    public GameObject addBar;

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
    bool isproduce;

    public override void init()
    {
        base.init();
        ismater2 = (bool)message[0];
        curid = (int)message[1];
        curcount = (int)message[2];
        isproduce = curid != 0;
        addBar.SetActive(false);
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
        cancelGring.onClick.AddListener(clickCancelGring);
        //监听事件      实时刷新现有数据
        EventAction.Instance.AddEventGather<bool, int>(eventType.millTimerMater_BI, refreshCount);
    }
    public override void unregisterEvent()
    {
        base.unregisterEvent();
        EventAction.Instance.RemoveAction<bool, int>(eventType.millTimerMater_BI, refreshCount);
    }
    void refreshMainmessage()
    {
        if (curid == 0)
        {
            itemImg.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), ConfigConst.NOIMG_ICON);
            cancelGring.gameObject.SetActive(false);
            gringding.text = "待选择要研磨的材料";
        }
        else
        {
            itemImg.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(curid).iconName);
            cancelGring.gameObject.SetActive(true);
            gringding.text = "研磨中...";
        }
    }

    void refreshCount(bool mater2,int count)
    {
        if (ismater2 == mater2 && isproduce)
        {
            curcount = count;
            contextcount.text = curcount.ToString();
            if (count == 0)
            {
                curid = 0;
                isproduce = false;
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
        //详细
    }
    void clickCancelGring()
    {
        //发消息
        if (ismater2)
            EventAction.Instance.TriggerAction(eventType.millAddMater2_II, curid, -curcount);
        else
            EventAction.Instance.TriggerAction(eventType.millAddMater1_II, curid, -curcount);
        curid = 0;
        curcount = 0;
        isproduce = false;
        refreshMainmessage();
    }
    void onChooseOne(int id)
    {
        curid = id;
        curcount = 0;
        gringding.text = "";
        addingContext.text = curcount.ToString();
        addBar.SetActive(true);
    }
    #endregion
}

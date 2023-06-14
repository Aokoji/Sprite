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
    public UITool_ScrollView scroll;        //变化型scroll
    public GameObject clone;
    public Button back;
    public Text gringding;  //研磨中字样
    public Button cancelGring;  //取消研磨
    public Button confirmGring;

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
        if (curcount == 0)
            curid = 0;
        else
            contextcount.text = curcount.ToString();
        isproduce = curid != 0;
        addBar.SetActive(false);
        clone.SetActive(false);
        scroll.initConfig(80, 80, clone);
        RunSingel.Instance.runTimer(initScroll());
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
        itemImg.onClick.AddListener(clickChooseIcon);
        cancelGring.onClick.AddListener(clickCancelGring);
        confirmGring.onClick.AddListener(clickConfirm);
        //监听事件      实时刷新现有数据
        EventAction.Instance.AddEventGather<bool, int>(eventType.millTimerMater_BI, refreshCount);
    }
    public override void unregisterEvent()
    {
        base.unregisterEvent();
        EventAction.Instance.RemoveAction<bool, int>(eventType.millTimerMater_BI, refreshCount);
    }
    IEnumerator initScroll()
    {
        var dat = PlayerManager.Instance.playerItemDic;
        GameObject obj;
        foreach(var item in dat)
        {
            if (Config_t_items.getOne(item.Key).type == 2)
            {
                obj = scroll.addItemDefault();
                var script = obj.GetComponent<BagItem>();
                script.setData(item.Key, item.Value);
                script.initAction(onChooseOne);
            }
        }
        scroll.reCalculateHeigh();
        yield return null;
    }
    //刷新添加板
    void refreshMainmessage()
    {
        Debug.Log("isinthere" + curid + isproduce);
        if (curid == 0)
        {
            itemImg.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), ConfigConst.NOIMG_ICON);
            cancelGring.gameObject.SetActive(false);
            gringding.text = "待选择要研磨的材料";
            addBar.SetActive(false);
        }
        else
        {
            if (isproduce)
            {
                cancelGring.gameObject.SetActive(true);
                gringding.text = "研磨中...";
                addBar.SetActive(false);
            }
            else
            {
                cancelGring.gameObject.SetActive(false);
                gringding.text = "";
                addBar.SetActive(true);
            }
            itemImg.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(curid).iconName);
        }
    }
    //计时器刷新现有研磨
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
        int nowhave = PlayerManager.Instance.getItem(curid);
        if (nowhave < curcount)
            curcount = nowhave;
        addingContext.text = curcount.ToString();
    }
    void clickConfirm()
    {
        if (curcount == 0)
        {
            PanelManager.Instance.showTips3("没有添加材料");
            return;
        }
        RunSingel.Instance.getBeiJingTime(result =>
        {
            //扣材料
            PlayerManager.Instance.addItems(curid, -curcount);
            //刷新mater
            if (ismater2)
            {
                PlayerManager.Instance.createMillMater2(curid, curcount, result);
            }
            else
            {
                PlayerManager.Instance.createMillMater1(curid, curcount, result);
            }
            PanelManager.Instance.DisposePanel();
        });
    }
    void clickChooseIcon()
    {
        //详细

    }
    //取消生产
    void clickCancelGring()
    {
        //获得未完成
        PlayerManager.Instance.addItems(curid, curcount);
        //更新mater
        //发消息   避免mill发消息
        if (ismater2)
            PlayerManager.Instance.addMillMater2(-curcount);
        else
            PlayerManager.Instance.addMillMater1(-curcount);
        EventAction.Instance.TriggerAction(eventType.millShutMater);
        RunSingel.Instance.runTimer(initScroll());
        //弹窗
        List<ItemData> list = new List<ItemData>
        {
            new ItemData(curid, curcount)
        };
        PanelManager.Instance.showTips4(list);

        curid = 0;
        curcount = 0;
        isproduce = false;
        refreshMainmessage();
    }
    void onChooseOne(int id)
    {
        //if(curid!=0)
        curid = id;
        curcount = 0;
        addingContext.text = curcount.ToString();
        refreshMainmessage();
    }
    #endregion
}

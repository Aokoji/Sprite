using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class MillPanel : PanelBase
{
    const string guide = "MILL_GUIDE";

    public Button mater1;   //材料箱
    public Button mater2;
    public Button collect1; //收集箱
    public Button collect2;
    public Button workstool1;   //岗位
    public WorkMessageBar workNode1;
    public WorkMessageBar workNode2;
    public Button workstool2;
    public Text materTime1;
    public Text materCount1;
    public Text collectCount1;
    public Text materTime2;
    public Text materCount2;
    public Text collectCount2;
    public GameObject mill2;

    public GameObject upgradeBar;
    public Text curLevel;
    public Text mater1mess;
    public Text mater2mess;
    public Text extramess;
    public Button barback;
    public Button upgrade;  //升级

    public Button cancel;

    bool timerLock;
    MillData _data;
    bool eventlock1;
    bool eventlock2;
    bool messAllow;
    bool messActive;
    bool flowBar;

    public override void registerEvent()
    {
        base.registerEvent();
        mater1.onClick.AddListener(clickmater1);
        mater2.onClick.AddListener(clickmater2);
        collect1.onClick.AddListener(clickCollect1);
        collect2.onClick.AddListener(clickCollect2);
        upgrade.onClick.AddListener(clickUpgrade);
        barback.onClick.AddListener(clickBarBack);
        cancel.onClick.AddListener(PanelManager.Instance.DisposePanel);
        EventAction.Instance.AddEventGather(eventType.millShutMater, refreshMaterMill);
        EventAction.Instance.AddEventGather<bool,int,int>(eventType.millChange_BII, millchangeAction);
    }
    public override void init()
    {
        base.init();
        _data = PlayerManager.Instance.Milldata;
        eventlock1 = true;
        eventlock2 = true;
        messAllow = true;
        messActive = false;
        workNode1.init(e_workSquare.workmill);
        workNode2.init(e_workSquare.workmill2);
        refreshBuilders();
    }
    public override void afterAnimComplete()
    {
        base.afterAnimComplete();
        if (GameManager.isOpenGuide)
            if (PlayerPrefs.GetInt(guide, 0) == 0)
                startGuide();
    }
    public override void reshow()
    {
        base.reshow();
        eventlock1 = true;
        eventlock2 = true;
        refreshBuilders();
    }
    void refreshBuilders()
    {
        initNormalData();
        //判断时间
        RunSingel.Instance.getBeiJingTime(result =>
        {
            refreshSpriteData(result);
            initDynamicData(result);
        });
    }
    void initNormalData()
    {
        timerLock = true;
        timeflow = 0;
        materTime1.gameObject.SetActive(false);
        materTime2.gameObject.SetActive(false);
        materCount1.text = "<空>";
        collectCount1.text = "<空>";
        if (_data.isupgrade)
        {
            collectCount2.text = "<空>";
            materCount2.text = "<空>";
        }
        mill2.SetActive(_data.isupgrade);
    }
    void initDynamicData(DateTime nowatime)
    {
        //判断时间
        if (_data.pdid1 > 0)
        {
            if (nowatime >= _data.getDate(_data.endtime1))
            {
                //收集
                col1 = _data.pdnum1;
            }
            else
            {
                //填料收集
                //启动计时器
                timerLock = false;
                surplus1 = (int)(_data.getDate(_data.endtime1) - nowatime).TotalSeconds;
                coef1 = Config_t_crop.getOne(_data.pdid1).produceCoef;
                pd1 = surplus1 / coef1 + 1;
                col1 = _data.pdnum1 - pd1;
            }
            resetMater1Panel();
        }
        if (_data.pdid2 > 0)
        {
            if (nowatime >= _data.getDate(_data.endtime2))
            {
                col2 = _data.pdnum2;
            }
            else
            {
                //填料收集
                timerLock = false;
                surplus2 = (int)(_data.getDate(_data.endtime2) - nowatime).TotalSeconds;
                coef2 = Config_t_crop.getOne(_data.pdid2).produceCoef;
                pd2 = surplus2 / coef2 + 1;
                col2 = _data.pdnum2  - pd2;
            }
            resetMater2Panel();
        }
    }
    void refreshSpriteData(DateTime time)
    {
        workNode1.refreshData(time);
        if (_data.isupgrade)
        {
            workNode2.gameObject.SetActive(true);
            workNode2.refreshData(time);
        }
        else 
            workNode2.gameObject.SetActive(false);
    }
    #region 操作
    //引导
    void startGuide()
    {

    }
    //填料
    void clickmater1()
    {
        //弹界面
        PanelManager.Instance.OpenPanel(E_UIPrefab.MillAdditionPanel, new object[] { false, _data.pdid1, pd1 });
        eventlock1 = false;
    }
    void clickmater2()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.MillAdditionPanel, new object[] { false, _data.pdid2, pd2 });
        eventlock2 = false;
    }
    //收获
    void clickCollect1()
    {
        if (_data.pdid1 == 0 || col1 == 0) return;
        List<ItemData> list = new List<ItemData>();
        list.Add(new ItemData(Config_t_crop.getOne(_data.pdid1).finishID, col1));
        PlayerManager.Instance.collectMill1(col1);
        PanelManager.Instance.showTips4(list);
        col1 = 0;
        resetMater1Panel();
    }
    void clickCollect2()
    {
        if (_data.pdid2 == 0 || col2 == 0) return;
        List<ItemData> list = new List<ItemData>();
        list.Add(new ItemData(Config_t_crop.getOne(_data.pdid1).finishID, col2));
        PlayerManager.Instance.collectMill2(col2);
        PanelManager.Instance.showTips4(list);
        col2 = 0;
        resetMater2Panel();
    }

    void clickBarBack()
    {
        if (!messAllow) return;
        if (messActive)
        {
            //隐藏
            messAllow = false;
            messActive = false;
            AnimationTool.playAnimation(upgradeBar, "millUpgradeBarBack", false, () => { messAllow = true; });
        }
        else
        {
            //显示 更新信息
            messActive = true;
            var data = PlayerManager.Instance.Milldata;
            curLevel.text = "磨坊等级："+data.extendLv;
            mater1mess.text = "主仓储容量："+data.capMillCount1;
            if (data.extendLv <= 2)
                mater2mess.gameObject.SetActive(false);
            else
            {
                mater2mess.gameObject.SetActive(true);
                mater2mess.text = "副仓储容量：" + data.capMillCount2;
            }
            if (data.extendLv < 5)
                extramess.gameObject.SetActive(false);
            else
            {
                extramess.gameObject.SetActive(true);
                extramess.text = "生产速度+5%";
            }
            messAllow = false;
            AnimationTool.playAnimation(upgradeBar, "millUpgradeBarShow", false, () => { messAllow = true; });
        }
    }
    void clickUpgrade()
    {
        if(!messAllow) return;
        if(messActive)
            clickBarBack();
        //升级界面
        PanelManager.Instance.OpenPanel(E_UIPrefab.MillUpgradePanel);
    }

    void refreshMaterMill()
    {
        //断计时的
        pd1 = _data.pdnum1 - col1;
        if (pd1 <= 0)
        {
            //理论上=0
            surplus1 = 0;
            resetMater1Panel();
        }
        pd2 = _data.pdnum1 - col1;
        if (pd2 <= 0)
        {
            //理论上=0
            surplus2 = 0;
            resetMater2Panel();
        }
    }
    void millchangeAction(bool ismater2,int id,int count)
    {
        //扣材料
        PlayerManager.Instance.addItemsNosave(id, -count);
        RunSingel.Instance.getBeiJingTime(result =>
        {
            //刷新mater
            if (ismater2)
            {
                clickCollect2();
                PlayerManager.Instance.createMillMater2(id, count, result);
            }
            else
            {
                clickCollect1();
                PlayerManager.Instance.createMillMater1(id, count, result);
            }
            refreshBuilders();
        });

    }
    #endregion
    #region timer
    int pd1;    //生产数量
    int col1;   //收集数量
    int coef1;  //系数
    int surplus1; //剩余时间
    int pd2;
    int col2;
    int coef2;
    int surplus2;

    float timeflow;
    //功能：显示现在计时
    public override void OnUpdate()
    {
        if (!timerLock)
        {
            timeflow += Time.deltaTime;
            if (timeflow >= 1)
            {
                //time1秒
                if (pd1 > 0)
                {
                    //存在未完成
                    surplus1--;
                    if (surplus1 <= 0)
                    {
                        surplus1 = 0;
                        pd1 = 0;
                        col1 = _data.pdnum1;
                    }
                    else
                    {
                        pd1 = surplus1 / coef1 + 1;
                        col1= _data.pdnum1 - pd1;
                    }
                    if (!eventlock1)
                        EventAction.Instance.TriggerAction(eventType.millTimerMater_BI,false, pd1);
                    resetMater1Panel();
                }
                if (pd2 > 0)
                {
                    //存在未完成
                    surplus2--;
                    if (surplus2 <= 0)
                    {
                        surplus2 = 0;
                        pd2 = 0;
                        col2 = _data.pdnum2;
                    }
                    else
                    {
                        pd2 = surplus2 / coef2 + 1;
                        col2 = _data.pdnum2 - pd2;
                    }
                    if (!eventlock2)
                        EventAction.Instance.TriggerAction(eventType.millTimerMater_BI, true, pd2);
                    resetMater2Panel();
                }
                timeflow -= 1;
            }
        }
    }
    void resetMater1Panel()
    {
        if (pd1 == 0)
            materCount1.text = "<空>";
        else
            materCount1.text = pd1.ToString();
        if (col1 == 0)
            collectCount1.text = "<空>";
        else
            collectCount1.text = col1.ToString();
        if(surplus1==0)
            materTime1.gameObject.SetActive(false);
        else
        {
            materTime1.text = "预计完成：" + PubTool.timeTranslateSeconds(surplus1);
            materTime1.gameObject.SetActive(true);
        }
        //+++他们还负责图标的刷新
    }
    void resetMater2Panel()
    {
        if (pd2 == 0)
            materCount2.text = "<空>";
        else
            materCount2.text = pd2.ToString();
        if (col2 == 0)
            collectCount2.text = "<空>";
        else
            collectCount2.text = col2.ToString();
        if (surplus2 == 0)
            materTime2.gameObject.SetActive(false);
        else
        {
            materTime2.text = "预计完成：" + PubTool.timeTranslateSeconds(surplus2);
            materTime2.gameObject.SetActive(true);
        }
    }

    #endregion
    public override void Dispose()
    {
        base.Dispose();
    }
}

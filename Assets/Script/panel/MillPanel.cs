using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MillPanel : PanelBase
{
    const string guide = "MILL_GUIDE";

    public Button mater1;   //材料箱
    public Button mater2;
    public Button collect1; //收集箱
    public Button collect2;
    public Button workstool1;   //岗位
    public Button workstool2;
    public Text materTime1;
    public Text materCount1;
    public Text collectCount1;
    public Text workCount1; //工作详情
    public RectTransform phyimg1;
    public Text materTime2;
    public Text materCount2;
    public Text collectCount2;
    public Text workCount2;
    public RectTransform phyimg2;
    public GameObject mill2;

    public Button upgrade;  //升级

    bool timerLock;
    MillData _data;
    DateTime starttime; //同步时间

    public override void registerEvent()
    {
        base.registerEvent();
        mater1.onClick.AddListener(clickmater1);
        mater2.onClick.AddListener(clickmater2);
        collect1.onClick.AddListener(clickCollect1);
        collect2.onClick.AddListener(clickCollect2);
        workstool1.onClick.AddListener(clickWork1);
        workstool2.onClick.AddListener(clickWork2);
        upgrade.onClick.AddListener(clickUpgrade);
    }
    public override void init()
    {
        base.init();
        _data = PlayerManager.Instance.Milldata;
        refreshBuilders();
    }
    public override void afterAnimComplete()
    {
        base.afterAnimComplete();
        if (PlayerPrefs.GetInt(guide, 0) == 0)
            startGuide();
    }
    void refreshBuilders()
    {
        initNormalData();
        //刨除    引导，材料1，2为空
        if (PlayerPrefs.GetInt(guide, 0) == 0 || (_data.pdid1 == 0 && _data.pdid2 == 0))
            return;
        //判断时间
        RunSingel.Instance.getBeiJingTime(result =>
        {
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
        refreshSpriteData();
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
    void refreshSpriteData()
    {
        if (_data.workingID1 > 0)
        {
            var dt = PlayerManager.Instance.getSpriteData(_data.workingID1);
            workCount1.text = dt.phy_max + "/" + dt.phy_cur;
            //设置image  +++ phyimg1
            workCount1.gameObject.SetActive(true);
            phyimg1.gameObject.SetActive(true);
        }
        else
        {
            workCount1.gameObject.SetActive(false);
            phyimg1.gameObject.SetActive(false);
        }
        if (_data.workingID2 > 0)
        {
            var dt = PlayerManager.Instance.getSpriteData(_data.workingID2);
            workCount2.text = dt.phy_max + "/" + dt.phy_cur;
            //设置image  +++ phyimg2
            workCount2.gameObject.SetActive(true);
            phyimg2.gameObject.SetActive(true);
        }
        else
        {
            workCount2.gameObject.SetActive(false);
            phyimg2.gameObject.SetActive(false);
        }
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

        //测试
        addMaterCount1(8, 2);
    }
    void clickmater2()
    {
        addMaterCount2(8, 2);
    }
    //派遣工作
    void clickWork1()
    {
        //工作界面
    }
    void clickWork2()
    {

    }
    //收获
    void clickCollect1()
    {
        List<ItemData> list = new List<ItemData>();
        list.Add(new ItemData(Config_t_crop.getOne(_data.pdid1).finishID, col1));
        PlayerManager.Instance.collectMill1(col1);
        PanelManager.Instance.showTips4(list);
        col1 = 0;
        resetMater1Panel();
    }
    void clickCollect2()
    {
        List<ItemData> list = new List<ItemData>();
        list.Add(new ItemData(Config_t_crop.getOne(_data.pdid1).finishID, col2));
        PlayerManager.Instance.collectMill2(col2);
        PanelManager.Instance.showTips4(list);
        col2 = 0;
        resetMater2Panel();
    }

    void clickUpgrade()
    {
        //升级界面
    }

    void addMaterCount1(int spid, int count)
    {
        if(_data.pdid1>0)
        {
            if(_data.pdid1 == spid)
            {
                //正常加
                PlayerManager.Instance.addMillMater1(count);
                pd1 += count;
                surplus1 += count * coef1;
                timerLock = false;
            }
            else
            {
                //不正常现象
                PubTool.LogError("添加id不正确");
                return;
            }
        }
        else
        {
            //新增
            RunSingel.Instance.getBeiJingTime(result =>
            {
                PlayerManager.Instance.createMillMater1(spid, count, result);
                coef1 = Config_t_crop.getOne(spid).produceCoef;
                pd1 += count;
                surplus1 += count * coef1;
                timerLock = false;
            });
        }
    }
    void addMaterCount2(int spid, int count)
    {
        if (_data.pdid2 > 0)
        {
            if (_data.pdid2 == spid)
            {
                //正常加
                PlayerManager.Instance.addMillMater2(count);
                pd2 += count;
                surplus2 += count * coef2;
                timerLock = false;
            }
            else
            {
                //不正常现象
                PubTool.LogError("添加id不正确2");
                return;
            }
        }
        else
        {
            //新增
            RunSingel.Instance.getBeiJingTime(result =>
            {
                PlayerManager.Instance.createMillMater2(spid, count, result);
                coef2 = Config_t_crop.getOne(spid).produceCoef;
                pd2 += count;
                surplus2 += count * coef2;
                timerLock = false;
            });
        }
    }
    #endregion
    #region timer
    int pd1;    //生产
    int col1;   //收集
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

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

    float timernum;
    bool timerLock;
    MillData _data;

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
        if (PlayerPrefs.GetInt(guide, 0) == 0 || _data.pdid1 == 0 || _data.pdid2 == 0)
            return;
        RunSingel.Instance.getBeiJingTime(result =>
        {
            initDynamicData();
        });
    }
    void initNormalData()
    {
        timerLock = true;
        timernum = 0;
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
    void initDynamicData()
    {
        timernum = 0;
        //启动计时器
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
    void setpackageNow()
    {

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
    }
    void clickmater2()
    {

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

    }
    void clickCollect2()
    {

    }

    void clickUpgrade()
    {
        //升级界面
    }
    #endregion
    //功能：显示现在计时
    private void Update()
    {
        
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}

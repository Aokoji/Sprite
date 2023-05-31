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

    public Button upgrade;  //升级

    float timernum;
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
        initData();
    }
    public override void afterAnimComplete()
    {
        base.afterAnimComplete();
        if (PlayerPrefs.GetInt(guide, 0) == 0)
            startGuide();
    }
    void initData()
    {
        timernum = 0;
        _data = PlayerManager.Instance.playerdata.mill;
        _data.extendLv = 2;
    }
    #region 操作
    //引导
    void startGuide()
    {

    }
    //填料
    void clickmater1()
    {

    }
    void clickmater2()
    {

    }
    //派遣工作
    void clickWork1()
    {

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

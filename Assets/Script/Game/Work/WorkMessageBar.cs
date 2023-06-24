using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkMessageBar : UIBase
{
    public Button icon;
    public GameObject stoneIcon;
    public Text workcontext;
    public Button cancelWork;
    public GameObject completeAnim;     //粒子效果或默认动画

    e_workSquare square;
    bool iscomplete;
    WorkData _data=null;
    bool isCancelshow;
    public void init(e_workSquare sq)
    {
        cancelWork.onClick.AddListener(clickCancel);
        icon.onClick.AddListener(clickIcon);
        square = sq;
        iscomplete = false;
        isCancelshow = false;
    }
    public bool checkGetTime()
    {
        _data = WorkManager.Instance.getSquareWork(square);
        return _data != null;
    }
    public void refreshData(DateTime result)
    {
        _data = WorkManager.Instance.getSquareWork(square);
        stoneIcon.SetActive(false);
        completeAnim.SetActive(false);
        cancelWork.gameObject.SetActive(false);
        if (_data != null)
        {
            icon.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(_data.spid).iconName);
            var time = _data.getDate(_data.endtime);
            if (time <= result)
            {
                //完成
                workcontext.text = "工作完成";
                iscomplete = true;
                completeAnim.SetActive(true);
            }
            else
            {
                workcontext.text = "剩余时间：" + PubTool.timeTranslate((int)(time - result).TotalMinutes);
                iscomplete = false;
                stoneIcon.SetActive(true);
            }
        }
        else
        {
            icon.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), ConfigConst.NOIMG_ICON);
            iscomplete = false;
            workcontext.text = "";
        }
    }
    void clickIcon()
    {
        if (iscomplete)
        {
            WorkManager.Instance.WorkFinish(square);
            refreshData(DateTime.Now);
        }
        else if (_data == null)
        {
            //空图标
            PanelManager.Instance.OpenPanel(E_UIPrefab.SpriteWorkPanel, new object[] { (int)square});
        }
        else
        {
            //出取消
            if (isCancelshow)
            {
                cancelWork.gameObject.SetActive(false);
                isCancelshow = false;
            }
            else
            {
                AnimationTool.playAnimation(gameObject, "workCancelShow");
                isCancelshow = true;
            }
        }
    }
    void clickCancel()
    {
        PanelManager.Instance.showTips2("确定取消正在进行的工作吗？", "（返还旅行消耗75%的体力）",()=>
        {
            WorkManager.Instance.WorkShut(square);
            refreshData(DateTime.Now);
        });
    }
}

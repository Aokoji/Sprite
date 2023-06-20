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
        refreshData();
    }
    void refreshData()
    {
        _data = WorkManager.Instance.getSquareWork(square);
        stoneIcon.SetActive(false);
        completeAnim.SetActive(false);
        if (_data != null)
        {
            RunSingel.Instance.getBeiJingTime(result =>
            {
                icon.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(_data.spid).iconName);
                var time = _data.getDate(_data.endtime);
                if (time > result)
                {
                    //完成
                    workcontext.text = "工作完成";
                    iscomplete = true;
                    completeAnim.SetActive(true);
                }
                else
                {
                    workcontext.text = "剩余时间：" + PubTool.timeTranslate((time - result).Seconds);
                    iscomplete = false;
                    stoneIcon.SetActive(true);
                }
            });
        }
        else
        {
            icon.GetComponent<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), ConfigConst.NOIMG_ICON);
            iscomplete = false;
        }
    }
    void clickIcon()
    {
        if (iscomplete)
        {

        }else if (_data == null)
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

    }
    void showCancel()
    {

    }
}

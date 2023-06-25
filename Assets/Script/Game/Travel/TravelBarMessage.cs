using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class TravelBarMessage : UIBase
{
    public GameObject travelingBar; //派出和待领取,超时同一个界面
    public GameObject questBar;
    public Text title;
    public Image rank;
    public Image[] icons;
    public Text[] contexts;

    public Image spTravelIcon;  //派遣头像
    public Text travelingContext;   //派遣中描述
    public Button travelShut;   //中断按钮
    public Button travelconfirm;   //确认按钮
    public Text alivetime;  //时间
    

    QuestData _data;
    bool iscomplete;
    DateTime curtimeJust;

    private void Start()
    {
        questBar.GetComponent<Button>().onClick.AddListener(click_lookMoreMessage);
    }

    public void reset(QuestData quest,DateTime cur)
    {
        iscomplete = false;
        curtimeJust = cur;
        _data = quest;
        refreshQuestCard();
    }
    //刷新单张委托卡
    void refreshQuestCard()
    {
        //状态5   空，派遣，归来，任务单,超时      交付中，领取奖励+++
        travelingBar.SetActive(false);
        questBar.SetActive(false);
        alivetime.gameObject.SetActive(true);
        if (curtimeJust > _data.getDate(_data.spFinish))
        {
            if (curtimeJust > _data.getDate(_data.endTime))
            {
                //超时
                travelingBar.SetActive(true);
                refresh_timeout();
            }
            else
            {
                if (_data.isGet)
                {
                    //普通委托单
                    questBar.SetActive(true);
                    refresh_normalQuest();
                }
                else
                {
                    //待领取（归来）
                    travelingBar.SetActive(true);
                    alivetime.gameObject.SetActive(false);
                    refresh_willReward();
                }
            }
        }
        else
        {
            //派遣中
            travelingBar.SetActive(true);
            refresh_traveling();
        }
 
    }
    void refresh_traveling()
    {
        spTravelIcon.gameObject.SetActive(true);
        spTravelIcon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_ActorMessage.getOne(_data.spID).titleIcon);
        travelShut.gameObject.SetActive(true);
        travelconfirm.gameObject.SetActive(false);
        travelingContext.text = "旅行中，预计返回时间：";
        alivetime.text = PubTool.timeTranslate((int)(_data.getDate(_data.spFinish) - curtimeJust).TotalMinutes);

        travelShut.onClick.RemoveAllListeners();
        travelShut.onClick.AddListener(click_backtravel);
    }
    void refresh_timeout()
    {
        spTravelIcon.gameObject.SetActive(false);
        travelShut.gameObject.SetActive(false);
        travelconfirm.gameObject.SetActive(true);
        travelingContext.text = Config_t_quest.getOne(_data.questID).sname;
        alivetime.text = "该任务已超时";

        travelconfirm.onClick.RemoveAllListeners();
        travelconfirm.onClick.AddListener(click_timeoutClear);
    }
    void refresh_willReward()
    {
        spTravelIcon.gameObject.SetActive(true);
        spTravelIcon.sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_ActorMessage.getOne(_data.spID).titleIcon);
        travelShut.gameObject.SetActive(false);
        travelconfirm.gameObject.SetActive(true);
        travelingContext.text = "旅行归来，请查看委托";
        alivetime.text = "";

        travelconfirm.onClick.RemoveAllListeners();
        travelconfirm.onClick.AddListener(click_backWithAward);
    }
    void refresh_normalQuest()
    {
        var data = Config_t_quest.getOne(_data.questID);
        title.text = data.sname;
        string[] str = data.need.Split('|');
        string[] strneed = data.needCount.Split('|');
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(false);
            contexts[i].gameObject.SetActive(false);
        }
        int comp=0;
        for (int i = 0; i < icons.Length; i++)
        {
            if (i < str.Length)
            {
                var item = Config_t_items.getOne(int.Parse(str[i]));
                icons[i].gameObject.SetActive(true);
                icons[i].sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), item.iconName);
                contexts[i].gameObject.SetActive(true);
                int num = int.Parse(strneed[i]);
                int have = PlayerManager.Instance.getItem(item.id);
                contexts[i].color = Color.black;
                if (have!=0)
                {
                    if (have >= num)
                    {
                        contexts[i].color = Color.green;
                        comp++;
                    }
                }
                contexts[i].text = num + "/" + have;
            }
            else
            {//空
                icons[i].gameObject.SetActive(false);
                contexts[i].gameObject.SetActive(false);
            }
        }
        if (comp >= str.Length)
            iscomplete = true;
    }
    #region clicks
    void click_backtravel()
    {
        //shut
        PanelManager.Instance.showTips2("确定召回旅行中的妖精吗？", "（返还旅行消耗75%的体力）", () => { TravelManager.Instance.shutTravel(_data); });
    }
    void click_timeoutClear()
    {
        TravelManager.Instance.clearTravel(_data);
        EventAction.Instance.TriggerAction(eventType.spriteTravelBackRefresh);
    }
    void click_backWithAward()
    {
        //获得奖励
        if (_data.takeItem.Count > 0)
        {
            foreach (var i in _data.takeItem)
                PlayerManager.Instance.addItemsNosave(i, 1);
        }
        if (_data.extraID > 0)
            PlayerManager.Instance.addItemsNosave(_data.extraID, 1);
        _data.isGet = true;
        PlayerManager.Instance.savePlayerData();
        TravelManager.Instance.clearTravel(_data);
        PanelManager.Instance.OpenPanel(E_UIPrefab.TravelBackQuestPanel, new object[] { _data });
    }
    void click_lookMoreMessage()
    {
        PanelManager.Instance.OpenPanel(E_UIPrefab.TravelMoreMessagePanel, new object[] { _data.questID, iscomplete });
    }
    #endregion
}

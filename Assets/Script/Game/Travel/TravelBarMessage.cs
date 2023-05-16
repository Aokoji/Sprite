using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelBarMessage : UIBase
{
    public Text title;
    public Text rank;
    public Image[] icons;
    public Text[] contexts;
    //状态有点麻烦 需要理一理

    QuestData _data;
    bool iscomplete;
    DateTime curtimeJust;

    public void reset(QuestData quest,DateTime cur)
    {
        iscomplete = false;
        curtimeJust = cur;
        _data = quest;
        refreshQuestCard();
    }

    void refreshQuestCard()
    {
        //状态5   空，派遣，归来，任务单,超时
        var data = Config_t_quest.getOne(_data.questID);
        title.text = data.sname;
        string[] str = data.need.Split('|');
        string[] strneed = data.needCount.Split('|');
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(false);
            contexts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < icons.Length; i++)
        {
            if(i< str.Length)
            {
                var item = Config_t_items.getOne(int.Parse(str[i]));
                icons[i].gameObject.SetActive(true);
                icons[i].sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), item.iconName);
                contexts[i].gameObject.SetActive(true);
                int num = int.Parse(strneed[i]);
                int have = 0;
                if (PlayerManager.Instance.playerItemDic.ContainsKey(item.id))
                {
                    have = PlayerManager.Instance.playerItemDic[item.id];
                    if (have >= num)
                        iscomplete = true;
                }
                contexts[i].text = num + "/" + have;
            }
            else
            {//空
                icons[i].gameObject.SetActive(false);
                contexts[i].gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelMoreMessagePanel : PanelTopBase
{
    public Text questtitle;
    public Text questdes;
    public Text questFrom;
    public Text questActor;
    public Image[] icons;
    public Text[] contexts;
    public Button completeBtn;
    public Button closeBtn;

    bool iscomplete;
    bool clickallow;
    t_quest _data;

    //这个界面只需要init
    public override void init()
    {
        int id = (int)message[0];
        _data = Config_t_quest.getOne(id);
        iscomplete = (bool)message[1];
        clickallow = true;
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(false);
            contexts[i].gameObject.SetActive(false);
        }
        questtitle.text = _data.sname;
        questdes.text = _data.describe;
        questFrom.text = Config_t_TravelRandom.getOne(_data.fromSquare).sname;
        questActor.text = _data.fromActor;
        string[] str = _data.need.Split('|');
        string[] strneed = _data.needCount.Split('|');
        for (int i = 0; i < icons.Length; i++)
        {
            if (i < str.Length)
            {
                var item = Config_t_items.getOne(int.Parse(str[i]));
                icons[i].gameObject.SetActive(true);
                icons[i].sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), item.iconName);
                contexts[i].gameObject.SetActive(true);
                int num = int.Parse(strneed[i]);
                int have = 0;
                contexts[i].color = Color.black;
                if (PlayerManager.Instance.playerItemDic.ContainsKey(item.id))
                {
                    have = PlayerManager.Instance.playerItemDic[item.id];
                    if (have >= num)
                    {
                        contexts[i].color = Color.green;
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
    }
    public override void registerEvent()
    {
        base.registerEvent();
        completeBtn.onClick.AddListener(clickComplete);
        closeBtn.onClick.AddListener(PanelManager.Instance.DisposePanel);
    }

    void clickComplete()
    {
        if (!clickallow) return;
        if (!iscomplete)
        {
            PanelManager.Instance.showTips3("委托未完成");
            return;
        }
        clickallow = false;
        //奖励
        List<ItemData> rewards = new List<ItemData>();
        string[] str;
        string[] stnum;
        if (!_data.rewards.Equals("0"))
        {
            str = _data.rewards.Split('|');
            stnum = _data.rewardnum.Split('|');
            for(int i=0;i< str.Length; i++)
            {
                var item = new ItemData(int.Parse(str[i]), int.Parse(stnum[i]));
                rewards.Add(item);
            }
        }
        PlayerManager.Instance.addItems(rewards);
        PanelManager.Instance.showTips4(rewards);
        TravelManager.Instance.completeRemoveQuest(_data.id);
        PanelManager.Instance.DisposePanel();
    }
}

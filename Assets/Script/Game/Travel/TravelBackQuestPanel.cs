using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//归来领取奖励和显示品级界面
public class TravelBackQuestPanel : PanelTopBase
{
    public GameObject backQuestRank;
    public Text questTitle;
    public GameObject[] awarditem;   //仨
    public GameObject rareParticle; //稀有特效
    public Button confirm;

    QuestData _data;
    int pointindex;
    bool allow;

    public override void init()
    {
        rareParticle.SetActive(false);
        backQuestRank.SetActive(false);
        pointindex = 0;
        allow = false;
        _data = message[0] as QuestData;
        foreach (var i in awarditem)
            i.gameObject.SetActive(false);
        //显示奖励
        if (_data == null) return;
        if (_data.takeItem.Count > 0)
        {
            foreach(var i in _data.takeItem)
            {
                awarditem[pointindex].GetComponentInChildren<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(i).iconName);
                awarditem[pointindex].gameObject.SetActive(true);
                pointindex++;
            }
        }
        if (_data.extraID > 0)
        {
            awarditem[pointindex].GetComponentInChildren<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(_data.extraID).iconName);
            awarditem[pointindex].gameObject.SetActive(true);
            rareParticle.SetActive(true);
            rareParticle.transform.position = awarditem[pointindex].transform.position;
            pointindex++;
        }
    }
    public override void registerEvent()
    {
        base.registerEvent();
        confirm.onClick.AddListener(clickClose);
    }
    public override void afterAnimComplete()
    {
        base.afterAnimComplete();
        if (_data.takeItem.Count > 0 || _data.extraID > 0)
            AnimationTool.playAnimation(backQuestRank, "stampShowRankReward",false,()=> { allow = true; });
        else
            AnimationTool.playAnimation(backQuestRank, "stampShowRank", false, () => { allow = true; });
    }
    void clickClose()
    {
        if (!allow) return;
        allow = false;
        //显示奖励
        if (_data.takeItem.Count > 0 || _data.extraID > 0)
        {
            if (_data.extraID > 0)
                _data.takeItem.Add(_data.extraID);
            List<ItemData> list = new List<ItemData>();
            foreach(var i in _data.takeItem)
                list.Add(new ItemData(i, 1));
            PanelManager.Instance.showTips4(list);
        }
        PanelManager.Instance.DisposePanel();
    }
}

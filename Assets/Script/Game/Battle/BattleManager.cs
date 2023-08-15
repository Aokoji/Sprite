using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;
public class BattleManager : CSingel<BattleManager>
{
    BattleControl ctrl;
    explorIcon curtype;
    List<ItemData> result;

    public void init()
    {
        refreshBattleData();
    }

    public void EnterBattle(int enemy, bool ischange = false, explorIcon stype=explorIcon.battle)
    {
        ctrl = new BattleControl();
        curtype = stype;
        refreshBattleData();
        ctrl.newbattle(enemy, curtype, ischange);
    }

    private void refreshBattleData()
    {
        result = null;
    }

    public void endreward(int id)
    {
        //获得物品
        result = new List<ItemData>();

        var data = Config_t_ActorMessage.getOne(id);
        System.Random random = new System.Random();
        int num;
        if (data.volume == 1 || data.volume == 0) 
            num = data.volume;
        else
            num = random.Next(-1, 1) + data.volume;
        //稀有奖励
        if (random.Next(1000) <= data.odds)
        {
            num--;
            string[] rare = data.dropRare.Split('|');
            var item = new ItemData(int.Parse(rare[random.Next(rare.Length)]), random.Next(10)<=2?2:1);
            item.rare = 1;
            result.Add(item);
        }
        string[] str = data.drop.Split('|');
        Dictionary<int, int> list = new Dictionary<int, int>();
        int index;
        //普通奖励
        for(int i = 0; i < num; i++)
        {
            index = int.Parse(str[random.Next(str.Length)]);
            if (!list.ContainsKey(index))
                list[index] = 0;
            list[index]++;
        }
        //+++固定奖励
        str = data.dropCommon.Split('|');
        foreach(var i in str)
        {
            index = int.Parse(i);
            if (!list.ContainsKey(index))
                list[index] = 0;
            list[index]++;
        }
        //整合dic
        foreach (var i in list)
        {
            result.Add(new ItemData(i.Key, i.Value));
        }
        //获得boss每日奖励
        if (curtype == explorIcon.boss)
        {
            var explor = PlayerManager.Instance.getExplorData();
            if (explor.dayboss > 0)
            {
                explor.dayboss_short = explor.dayboss;
                explor.dayboss = 0;
                PlayerManager.Instance.addItemsNosave(explor.dayboss_short, 1);
            }
            else
            {
                explor.dayboss_short = 0;
            }
        }
        else
        {
            //普通奖励
        }
        PlayerManager.Instance.addItems(result);
    }
    public void settleStep(bool iswin)
    {
        PanelManager.Instance.panelUnlock();
        if (iswin )
        {
            PanelManager.Instance.showTips5("获得素材", result, () =>
            {
                EventAction.Instance.TriggerAction(eventType.explorGoNextRank_B, true);
                ctrl.dispose();
                ctrl = null;
            });
        }
        else
        {
            //直接返回
            EventAction.Instance.TriggerAction(eventType.explorGoNextRank_B, false);
            ctrl.dispose();
            ctrl = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;
public class BattleManager : CSingel<BattleManager>
{
    BattleControl ctrl;         //目前未做销毁    +++
    public int battleEnemyID;
    explorIcon curtype;
    List<ItemData> result;

    public void init()
    {
        refreshBattleData();
    }

    public void EnterBattle(int enemy,explorIcon stype=explorIcon.battle)
    {
        ctrl = new BattleControl();
        curtype = stype;
        refreshBattleData();
        ctrl.newbattle(enemy);
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
        int num = random.Next(-1, 1) + data.volume;
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
        for(int i = 0; i < num; i++)
        {
            index = int.Parse(str[random.Next(str.Length)]);
            if (list.ContainsKey(index))
                list[index] = 0;
            list[index]++;
        }
        foreach(var i in list)
            result.Add(new ItemData(i.Key, i.Value));

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
        PlayerManager.Instance.addItems(result);
    }
    public void settleStep(bool iswin)
    {
        PanelManager.Instance.panelUnlock();
        if (iswin )
        {
            PanelManager.Instance.showTips5("获得奖励", result, () =>
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

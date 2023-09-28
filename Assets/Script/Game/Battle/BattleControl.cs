using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class BattleControl :Object
{
    private BattlePanel ui;
    public bool loadSuccess = false;
    bool dochange;
    #region 获取数据  加载的准备阶段
    public void newbattle(int en,explorIcon stype,bool ischange)
    {
        loadSuccess = false;
        dochange = ischange;
        registerEvent();
        getPlayerCardData();
        player = PlayerManager.Instance.getcursprite().Copy();
        enemy = EnemyCalculate.GetEnemyData(en);
        if (stype == explorIcon.boss)
        {
            enemy.hp_max += 10;
            enemy.hp_cur = enemy.hp_max;
            enemy.cost_max = enemy.cost_cur = 4;
            enemy.extraLimit = 1;
        }
        if (stype == explorIcon.elite)
        {
            enemy.hp_max += 5;
            enemy.hp_cur = enemy.hp_max;
        }
        if (stype == explorIcon.monster)
        {
            enemy.hp_max += 20;
            enemy.hp_cur = enemy.hp_max;
            enemy.cost_max = enemy.cost_cur = 4;
            enemy.extraLimit = 1;
        }
        if (stype == explorIcon.lord)
        {
            enemy.hp_max *= 2;
            enemy.hp_cur = enemy.hp_max;
            enemy.cost_max = enemy.cost_cur = 5;
            enemy.extraLimit =2;
        }
        player.refreshData();
        enemy.refreshData();
        RunSingel.Instance.runTimer(loadtimer());
    }
    IEnumerator loadtimer()
    {
        createPanel();
        while (!loadSuccess)
            yield return null;
        //播放开始动画 或者战斗信息
        StartRound();
    }
    private void registerEvent()
    {
        EventAction.Instance.AddEventGather<List<CardEntity>>(eventType.roundEnd_C, settleRoundAction);
        EventAction.Instance.AddEventGather(eventType.playRoundNext, roundNext);
        EventAction.Instance.AddEventGather(eventType.panelChangeLoadingComplete, loadPanelComplete);
    }
    void unregisterEvent()
    {
        EventAction.Instance.RemoveAction<List<CardEntity>>(eventType.roundEnd_C, settleRoundAction);
        EventAction.Instance.RemoveAction(eventType.playRoundNext, roundNext);
        EventAction.Instance.RemoveAction(eventType.panelChangeLoadingComplete, loadPanelComplete);
    }
    private void getPlayerCardData()
    {

    }
    private void createPanel()
    {
        if(dochange)
            PanelManager.Instance.ChangePanel(E_UIPrefab.BattlePanel);
        else
            PanelManager.Instance.OpenPanel(E_UIPrefab.BattlePanel);
    }
    void loadPanelComplete()
    {
        if (PanelManager.Instance.curEnmu != E_UIPrefab.BattlePanel.ToString()) return;
        ui = PanelManager.Instance.PanelCur.gameObject.GetComponent<BattlePanel>();
        ui.initData(enemy, player);
        loadSuccess = true;
    }
    #endregion

    private SpriteData player;
    private SpriteData enemy;
    #region  游戏流程
    //流程，发牌5：操作 出牌，抽牌
    private int round;  //回合计数
    private Queue<bool> willTake = new Queue<bool>();      //队列
    Queue<RoundData> willTakeplayerque = new Queue<RoundData>();
    Queue<RoundData> willTakeenemyque = new Queue<RoundData>();

    short pContinuous;      //连击计数
    short eContinuous;
    bool iscounterP;        //反制
    bool iscounterE;
    bool ispowerP;  //秘术
    bool ispowerE;
    int isReturnP;  //反伤   百分比
    int isReturnE;

    bool isArcaneOn;    //奥术强化  仅玩家
    bool isEtch;    //仅enemy

    bool firstplayer;//先后手
    bool iswin;

    public void StartRound()
    {
        //回合开始 发牌
        firstplayer = true;
        ui.startGame();
    }


    //出牌结束 开始统计回合
    private void settleRoundAction(List<CardEntity> datas)
    {
        var endata = ui.getEnemyround(enemy);
        RoundData rounddata;
        Queue<CardEntity> playerque = new Queue<CardEntity>();
        Queue<CardEntity> enemyque = new Queue<CardEntity>();
        datas.ForEach(item=>{
            rounddata = new RoundData();
            rounddata.entity = item;
            rounddata.isBasic = !item.isextra;
            rounddata._card = rounddata.entity._data;
            rounddata.isplayer = true;
            willTakeplayerque.Enqueue(rounddata);
        });
        endata.ForEach(item=>{
            rounddata = new RoundData();
            rounddata.entity = item;
            rounddata._card = rounddata.entity._data;
            rounddata.isplayer = false;
            willTakeenemyque.Enqueue(rounddata);
        });
        round = 0;
        pContinuous = 0;
        eContinuous = 0;
        iscounterP = false;
        iscounterE = false;
        ui.removeBuff(5);
        ui.removeBuff(5, true);
        isEtch = false;
        isReturnP = 0;
        isReturnE = 0;
        ispowerE = willTakeenemyque.Count == 1;
        ispowerP = willTakeplayerque.Count == 1;
        if (willTakeenemyque.Count+ willTakeplayerque.Count > 0)
        {
            isplayerround = firstplayer;
            ui.playRoundWillShow();
        }
        else
            PubTool.LogError("无人出牌 处理一下逻辑问题。");
    }
    //      ----------------------------        *******循环体*******       --------------------------------------------
    private bool isplayerround;
    private void roundNext()
    {
        if (!gamecheck())
        {
            gamesettle();
            return;
        }
        //结束检测
        if (willTakeenemyque.Count + willTakeplayerque.Count <= 0)
        {
            endRoundSettle();
            return;
        }
        //获取数据
        round++;
        RoundData data;
        if(willTakeplayerque.Count<=0) isplayerround=false;
        if(willTakeenemyque.Count<=0) isplayerround=true;
        if (isplayerround)
            data = willTakeplayerque.Dequeue();
        else
            data = willTakeenemyque.Dequeue();
        //计算反制
        if ((isplayerround && iscounterP)|| (!isplayerround && iscounterE))
        {
            //还原值
            if (isplayerround) iscounterP = false;
            else iscounterE = false;
            if (data._card.type2 == CardType2.d_decounter || data._card.conditionType1==CardType2.d_decounter || data._card.conditionType2 == CardType2.d_decounter || data._card.conditionType3 == CardType2.d_decounter)
            {
                data.isdecounter = true;
                data.isCounter = false;
            }
            else
            {
                data.isCounter = true;
                isplayerround = !isplayerround;
                continuousShut(data.isplayer);
                playCardNext(data);
                return;
            }
        }
        if(isplayerround && isEtch)
        {
            data.etch = true;
            continuousShut(data.isplayer);
            playCardNext(data);
            return;
        }
        //计算伤害
        data.hitnum = 0;
        switch (data._card.type2)
        {
            case CardType2.none:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                break;
            case CardType2.n_continuous:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                data.continuousnum = data.isplayer ? pContinuous : eContinuous;
                if ((data.isplayer && pContinuous >= 1)||(!data.isplayer && eContinuous>=1))
                {
                    conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                    conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                }
                break;
            case CardType2.n_thump:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                if ((data.isplayer && willTakeenemyque.Count<=0)||(!data.isplayer && willTakeplayerque.Count <= 0))
                {
                    conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                    conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                }
                break;
            case CardType2.e_deplete:
                data.hitnum = data._card.damage1;
                data.hitselfnum = data._card.damage2;
                conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                break;
            case CardType2.e_gift:
                for(int i = 0; i < data._card.damage1; i++)
                    data.gift.Add(CardCalculate.getRandomTypeCardList((CardSelfType)data._card.damage2));
                conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                break;
            case CardType2.d_power:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                if ((ispowerP && data.isplayer) || (ispowerE && !data.isplayer))
                {
                    conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                    conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                }
                break;
            case CardType2.d_decounter:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                if (data.isdecounter)
                {
                    conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                }
                break;
            case CardType2.s_blessup:
                if((data.isplayer && player.extraLimit>= data._card.damage1)||(!data.isplayer && enemy.extraLimit >= data._card.damage1))
                    conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                else
                    data.extraLimit = data._card.damage1;
                break;
            case CardType2.s_arcaneOff:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                if(isArcaneOn)
                    conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                break;
            default:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                break;
        }
        //****  增减伤计算   双方都要算
        buffCalculater(data);
        //反伤 结算
        if (data.hitnum > 0)
        {
            if (data.isplayer && isReturnE > 0)
            {
                data.hitselfnum += isReturnE / 100 * data.hitnum;
                isReturnE = 0;
            }
            if (!data.isplayer && isReturnP > 0)
            {
                data.hitselfnum += isReturnP / 100 * data.hitnum;
                isReturnP = 0;
            }
        }
        //连击计算
        if (data.hitnum == 0)
            continuousShut(data.isplayer);
        else
            continuousAdd(data.isplayer);
        firstplayer = !data.isplayer;

        //回合计算
        if(data._card.type2!=CardType2.n_preempt&&data._card.conditionType1!=CardType2.n_preempt&&data._card.conditionType2!=CardType2.n_preempt)
            isplayerround = !isplayerround;
        if (willTakeplayerque.Count <= 0) isplayerround = false;
        if (willTakeenemyque.Count <= 0) isplayerround = true;
        playCardNext(data);
    }

    //条件 check      后手词条
    private void conditionTypeCalculate(RoundData data,CardType2 type,int damage)
    {
        if (type == CardType2.none) return;
        int count;
        SpriteData from = data.isplayer ? player : enemy;
        switch (type)
        {
            case CardType2.none:
                break;
            case CardType2.n_hit:
                data.hitnum += damage;
                break;
            case CardType2.n_deal:
                data.dealnum += damage;
                break;
            case CardType2.n_defence:
                data.defnum += damage;
                break;
            case CardType2.n_recover:
                data.recovernum += damage;
                break;
            case CardType2.n_continuous:
                if ((!data.isplayer && eContinuous >= 1) || (data.isplayer && pContinuous>=1)) 
                    data.hitnum+= damage;
                data.continuousnum = data.isplayer ? pContinuous : eContinuous;
                break;
            case CardType2.n_thump:
                if ((data.isplayer && willTakeenemyque.Count <= 0) || (!data.isplayer && willTakeplayerque.Count <= 0))
                    data.hitnum += damage;
                break;
            case CardType2.n_preempt:
                data.hitnum += damage;
                break;
            case CardType2.e_deplete:
                data.hitnum += damage;
                data.hitselfnum += damage;
                break;
            case CardType2.e_gift:
                data.gift.Add(CardCalculate.getRandomTypeCardList((CardSelfType)damage));
                break;
            case CardType2.e_giftone:
                data.gift.Add(damage);
                break;
            case CardType2.e_giftTwo:
                data.gift.Add(damage);
                data.gift.Add(damage);
                break;
            case CardType2.e_giftThree:
                data.gift.Add(damage);
                data.gift.Add(damage);
                data.gift.Add(damage);
                break;
            case CardType2.e_addition:
                data.addition = damage;
                break;
            case CardType2.e_defend:
                if (data.isplayer) ui.AddBuff(5);
                else ui.AddBuff(5, true);
                break;
            case CardType2.n_counter:
                if (data.isplayer) iscounterE = true;
                else iscounterP = true;
                break;
            case CardType2.n_broke:
                data.brokenum += damage;
                break;
            case CardType2.s_todefen:
                //处理一下+++
                data.toDefen += damage;
                break;
            case CardType2.s_boost:
                if (data.isplayer) ui.AddBuff(2, false, damage);
                else ui.AddBuff(2, true, damage);
                break;
            case CardType2.g_return:
                if (data.isplayer) isReturnP = damage;
                else isReturnE = damage;
                break;
            case CardType2.g_umbra:
                if (damage >= 100 && damage < 200)
                {
                    //1类，损失的 百分比治疗
                    count = (damage - 100) / 100 * (from.hp_max - from.hp_cur);
                }
                if(damage>=200 && damage < 300)
                {
                    //2类，上限百分比治疗
                    count = (damage - 200) / 100 * from.hp_max;
                }
                else
                    count = damage;
                data.hitnum += count;
                data.recovernum += count;
                break;
            case CardType2.g_overcrue:
                if (damage >= 100 && damage < 200)                      //1类，恢复上限的过量伤害
                    count = (damage - 100) / 100 * from.hp_max;
                else
                    count = damage;
                if (from.hp_cur + count > from.hp_max)
                {
                    data.hitnum += count + from.hp_cur - from.hp_max;
                    data.recovernum += from.hp_max - from.hp_cur;
                }
                else
                    data.recovernum += count;
                break;
            case CardType2.s_etch:
                //侵蚀
                isEtch = true;
                break;
            case CardType2.s_arcaneOn:
                isArcaneOn = true;
                if (data.isplayer) ui.AddBuff(1);
                else ui.AddBuff(1, true);
                break;
            case CardType2.s_reduce:
                if (data.isplayer)
                    ui.AddBuff(3, true, damage);
                else
                    ui.AddBuff(3, false, damage);
                break;
            case CardType2.n_hittwo:
                data.morehit.Add(damage);
                data.morehit.Add(damage);
                break;
            case CardType2.n_hitthree:
                data.morehit.Add(damage);
                data.morehit.Add(damage);
                data.morehit.Add(damage);
                break;
        }
    }
    void effectBuff(t_Buff config, RoundData data, int num = 0)
    {
        switch (config.hittype)
        {
            
            case 1://非元素
                if(data.hitType==0)
                {
                    data.addition += num;
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
            case 12:
                if (data.hitnum > 0 || data.hitselfnum > 0 || data.morehit.Count > 0)
                {
                    data.hitnum = 0;
                    data.morehit.Clear();
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
            case 21://增强（全伤害
                if (data.hitnum > 0 || data.hitselfnum > 0 || data.morehit.Count>0)
                {
                    data.addition += num;
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
            case 30:
                if (data.hitnum > 0 || data.hitselfnum > 0 || data.morehit.Count > 0)
                {
                    data.addition -= num;
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
            case 31:
                if (data.hitType == 1)
                {
                    data.addition += num;
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
            case 32:
                if (data.hitType == 2)
                {
                    data.addition += num;
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
            case 33:
                if (data.hitType == 3)
                {
                    data.addition += num;
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
            case 34:
                if (data.hitType == 4)
                {
                    data.addition += num;
                    if (config.sustainType == 2 || config.sustainType == 3)
                        ui.removeBuff(config.id, !data.isplayer);
                }
                break;
        }
    }
    
    void buffCalculater(RoundData data)
    {
        t_Buff config;
        foreach(var i in ui.bufflistP)
        {
            config = Config_t_Buff.getOne(i.Key);
            if ((data.isplayer && config.taketype == 0) || (!data.isplayer && config.taketype == 1))
                effectBuff(config, data, i.Value);
        }
        foreach (var i in ui.bufflistE)
        {
            config = Config_t_Buff.getOne(i.Key);
            if ((!data.isplayer && config.taketype == 0) || (data.isplayer && config.taketype == 1))
                effectBuff(config, data, i.Value);
        }
    }
    private void playCardNext(RoundData data)
    {
        //结算这回合的 数据
        if (!data.isCounter)
        {
            if (data.isplayer)
                calculateYouTwoWTF(player, enemy, data);
            else
                calculateYouTwoWTF(enemy, player, data);
        }
        //播放这张的效果
        ui.playThisCard(data);
    }
    private void calculateYouTwoWTF(SpriteData take,SpriteData pass,RoundData data)
    {
        if(data.isplayer && data.etch)
        {
            if(data.isBasic)
                PlayerManager.Instance.etchCards(data._card.id);
            return;
        }
        if (pass.hp_cur + pass.def_cur <= data.hitnum)
            pass.hp_cur = 0;
        if (data.toDefen > 0)
        {
            data.toDefen += data.hit_addition;
            data.toDefen = Mathf.Max(0, data.toDefen);
            if (pass.def_cur > 0)
            {
                pass.def_cur -= data.toDefen;
                pass.def_cur = Mathf.Max(0, pass.def_cur);
            }
        }
        else if (data.hitnum > 0 || data.morehit.Count > 0)
        {
            int hit = 0;
            if (data.hitnum > 0)
            {
                data.hitnum += data.hit_addition;
                data.hitnum = Mathf.Max(0, data.hitnum);
            }
            hit = data.hitnum;
            foreach (var i in data.morehit)
                hit += i + data.hit_addition;
            if (pass.def_cur > 0)
            {
                if (hit > pass.def_cur)
                {
                    hit = hit - pass.def_cur;
                    pass.def_cur = 0;
                    pass.hp_cur -= hit;
                }
                else
                    pass.def_cur -= hit;
            }
            else
                pass.hp_cur -= hit;
        }
        else if (data.brokenum > 0)
        {
            data.brokenum += data.hit_addition;
            data.brokenum = Mathf.Max(0, data.brokenum);
            pass.hp_cur -= data.brokenum;
        }
        if (data.hitselfnum > 0)
        {
            data.hitselfnum += data.hit_addition;
            data.hitselfnum = Mathf.Max(0, data.hitselfnum);
            int hit = data.hitselfnum + data.hit_addition;
            if (take.def_cur > 0)
            {
                if (take.def_cur >= hit)
                    take.def_cur -= hit;
                else
                {
                    hit -= take.def_cur;
                    take.def_cur = 0;
                    take.hp_cur -= hit;
                }
            }
            else
                take.hp_cur -= hit;
        }
        if (data.recovernum > 0)
        {
            if (take.hp_cur + data.recovernum > take.hp_max)
            {
                data.recovernum = take.hp_max - take.hp_cur;
                take.hp_cur = take.hp_max;
            }
            else
                take.hp_cur += data.recovernum;
        }
        if (data.defnum > 0)
            take.def_cur += data.defnum;
        if (data.extraLimit > 0)
        {
            take.extraLimit = data.extraLimit;
            if (take.extraLimit == 1)
                take.cost_max = 4;
            if (take.extraLimit == 2)
                take.cost_max = 5;
        }
    }
    //单次回合结束
    private void endRoundSettle()
    {
        player.cost_cur = player.cost_max;
        enemy.cost_cur = enemy.cost_max;
        ui.roundEndAndContinue();
    }

    private void continuousAdd(bool isplayer)
    {
        if (isplayer) pContinuous++;
        else eContinuous++;
    }
    private void continuousShut(bool isplayer)
    {
        if (isplayer) pContinuous=0;
        else eContinuous=0;
    }

    private bool gamecheck()
    {//是否继续
        bool result = true;
        if (player.hp_cur <= 0)
        {
            player.hp_cur = 0;
            result = false;
            iswin = false;
        }
        if (enemy.hp_cur <= 0)
        {
            enemy.hp_cur = 0;
            result = false;
            iswin = true;
        }
        return result;
    }
    private void gamesettle()
    {//游戏结算
        if (iswin)
        {
            //计算enemy奖励
            EventAction.Instance.TriggerAction(eventType.battleFinish_I, enemy.id);
            BattleManager.Instance.endreward(enemy.id);
        }
        ui.gameSettle(iswin);
    }
    #endregion

    public void dispose()
    {
        unregisterEvent();
        PanelManager.Instance.DisposePanel();
    }
    #region  卡牌操作判断

    #endregion
}

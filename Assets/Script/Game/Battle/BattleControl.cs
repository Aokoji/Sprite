using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class BattleControl :Object
{
    private BattlePanel ui;
    public bool loadSuccess = false;

    #region 获取数据  加载的准备阶段
    public void newbattle()
    {
        getPlayerCardData();

        createPanel();
        registerEvent();
    }
    private void getPlayerCardData()
    {

    }
    private void createPanel()
    {
        PanelManager.Instance.ChangePanel(E_UIPrefab.BattlePanel, loadComplete);
    }

    private void registerEvent()
    {
        EventAction.Instance.AddEventGather<List<CardEntity>>(eventType.roundEnd_C, settleRoundAction);
        EventAction.Instance.AddEventGather(eventType.playRoundNext, roundNext);
    }

    private void loadComplete()
    {
        loadSuccess = true;
        ui = PanelManager.Instance.PanelCur.gameObject.GetComponent<BattlePanel>();
        player = PlayerManager.Instance.cursprite.Copy();
        enemy = EnemyCalculate.GetEnemyData();   //+++模拟一个数据
        player.refreshData();
        enemy.refreshData();
        ui.initData(enemy, player);
        //ui.refreshPlayerData(player);
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
    bool isdefendP;     //防护
    bool isdefendE;

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
        isdefendE = false;
        isdefendP = false;
        ispowerE = willTakeenemyque.Count == 1;
        ispowerP = willTakeplayerque.Count == 1;
        if (willTakeenemyque.Count+ willTakeplayerque.Count > 0)
        {
            isplayerround = firstplayer;
            ui.playRoundWillShow();
        }
        else
            Debug.LogError("无人出牌 处理一下逻辑问题。");
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
            if (data._card.type2 == CardType2.d_decounter )
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
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage3);
                break;
            case CardType2.e_gift:
                for(int i = 0; i < data._card.damage1; i++)
                    data.gift.Add(CardCalculate.getRandomTypeCardList((CardSelfType)data._card.damage2));
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
                if(data.isplayer && player.extraLimit>= data._card.damage1)
                    conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                else if (!data.isplayer && enemy.extraLimit >= data._card.damage1)
                    conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                else
                    data.extraLimit = data._card.damage1;
                break;
            default:
                conditionTypeCalculate(data, data._card.conditionType1, data._card.damage1);
                conditionTypeCalculate(data, data._card.conditionType2, data._card.damage2);
                conditionTypeCalculate(data, data._card.conditionType3, data._card.damage3);
                break;
        }
        //屏障 特殊结算
        if (data.hitnum > 0)
        {
            if(data.isplayer && isdefendE)
            {
                isdefendE = false;
                data.hitnum = 0;
                data.isdefend = true;
            }
            if (!data.isplayer && isdefendP)
            {
                isdefendP = false;
                data.hitnum = 0;
                data.isdefend = true;
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

    //条件 check
    private void conditionTypeCalculate(RoundData data,CardType2 type,int damage)
    {
        if (type == CardType2.none) return;
        switch (type)
        {
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
                if ((!data.isplayer && eContinuous >= 1) || (data.isplayer && pContinuous>=1)) data.hitnum+= damage;
                data.continuousnum = data.isplayer ? pContinuous : eContinuous;
                break;
            case CardType2.n_thump:
                if ((data.isplayer && willTakeenemyque.Count <= 0) || (!data.isplayer && willTakeplayerque.Count <= 0))
                    data.hitnum += damage;
                break;
            case CardType2.n_preempt:
                data.hitnum += damage;
                break;
            case CardType2.e_gift:
                for (int i = 0; i < damage; i++)
                    data.gift.Add(CardCalculate.getRandomTypeCardList(data._card.limit));
                break;
            case CardType2.e_giftone:
                data.gift.Add(damage);
                break;
            case CardType2.e_addition:
                data.addition = damage;
                break;
            case CardType2.e_defend:
                if (data.isplayer) isdefendP = true;
                else isdefendE = true;
                break;
            case CardType2.n_counter:
                if (data.isplayer) iscounterE = true;
                else iscounterP = true;
                break;
        }
    }

    private void playCardNext(RoundData data)
    {
        //结算这回合的 ***数据***
        if (data.isCounter)
        {
            ui.playThisCard(data);
            return;
        }
        if (data.isplayer)
            calculateYouTwoWTF(player, enemy, data);
        else
            calculateYouTwoWTF(enemy, player, data);
        //播放这张的效果
        ui.playThisCard(data);
    }
    private void calculateYouTwoWTF(SpriteData take,SpriteData pass,RoundData data)
    {
        if (pass.hp_cur + pass.def_cur <= data.hitnum)
            pass.hp_cur = 0;
        else if(data.hitnum>0)
        {
            int hit = data.hitnum;
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
        if (data.hitselfnum > 0)
        {
            if (take.def_cur > 0)
            {
                if (take.def_cur > data.hitselfnum)
                    take.def_cur -= data.hitselfnum;
                else
                {
                    int num = data.hitselfnum - take.def_cur;
                    take.def_cur = 0;
                    take.hp_cur -= num;
                }
            }
            else
                take.hp_cur -= data.hitselfnum;
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
        ui.gameSettle(iswin);
    }
    #endregion

    #region  卡牌操作判断

    #endregion
}

﻿using System.Collections;
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
        PanelManager.Instance.OpenPanel(E_UIPrefab.BattlePanel, loadComplete);
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
        player = PlayerManager.Instance.cursprite;
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

    bool iswin;

    public void StartRound()
    {
        //回合开始 发牌
        ui.startGame();
    }


    //出牌结束 开始统计回合
    private void settleRoundAction(List<CardEntity> datas)
    {
        var endata = ui.getEnemyround(enemy);
        RoundData rounddata;
        Queue<CardEntity> playerque = new Queue<CardEntity>();
        Queue<CardEntity> enemyque = new Queue<CardEntity>();
        datas.ForEach(item=>{ playerque.Enqueue(item); });
        endata.ForEach(item=>{ enemyque.Enqueue(item); });
        int countround = playerque.Count+enemyque.Count;
        bool isplayerround=true;
        for (int i = 0; i < countround; i++)
        {
            if (isplayerround)
            {
                if (playerque.Count <= 0) continue;
                willTake.Enqueue(isplayerround);
                rounddata = new RoundData();
                rounddata.entity = playerque.Dequeue();
                rounddata._card = rounddata.entity._data;
                rounddata.isplayer = true;
                if (rounddata._card.type2 != CardType2.n_preempt || playerque.Count <= 0)
                    isplayerround = false;
                if (enemyque.Count <= 0) isplayerround = true;
                willTakeplayerque.Enqueue(rounddata);
            }
            else
            {
                if (enemyque.Count <= 0) continue;
                willTake.Enqueue(isplayerround);
                rounddata = new RoundData();
                rounddata.entity = enemyque.Dequeue();
                rounddata._card = rounddata.entity._data;
                rounddata.isplayer = false;
                if (rounddata._card.type2 != CardType2.n_preempt || enemyque.Count <= 0)
                    isplayerround = true;
                if (playerque.Count <= 0) isplayerround = false;
                willTakeenemyque.Enqueue(rounddata);
            }
        }
        round = 0;
        pContinuous = 0;
        eContinuous = 0;
        iscounterP = false;
        iscounterE = false;
        isdefendE = false;
        isdefendP = false;
        ispowerE = willTakeenemyque.Count == 1;
        ispowerP = willTakeplayerque.Count == 1;
        if (willTake.Count > 0)
        {
            ui.playRoundWillShow();
        }
        else
            Debug.LogError("无人出牌 处理一下逻辑问题。");
    }
    //      ----------------------------        *******循环体*******       --------------------------------------------
    private void roundNext()
    {
        if (!gamecheck())
        {
            gamesettle();
            return;
        }
        //结束检测
        if (willTake.Count <=0)
        {
            endRoundSettle();
            return;
        }
        //获取数据
        round++;
        RoundData data;
        if (willTake.Dequeue())
            data = willTakeplayerque.Dequeue();
        else
            data = willTakeenemyque.Dequeue();
        //计算反制
        if ((data.isplayer && iscounterP)|| (!data.isplayer && iscounterE))
        {
            //还原值
            if (data.isplayer) iscounterP = false;
            else iscounterE = false;
            if (data._card.type2 == CardType2.e_decounter )
            {
                data.isdecounter = true;
                data.isCounter = false;
            }
            else
            {
                data.isCounter = true;
                continuousShut(data.isplayer);
                playCardNext(data);
                return;
            }
        }
        //计算伤害
        data.hitnum = 0;
        switch (data._card.type2)
        {
            case CardType2.n_hit:
                data.hitnum = data._card.damage1;
                continuousAdd(data.isplayer);
                break;
            case CardType2.n_preempt:
                data.hitnum = data._card.damage1;
                continuousAdd(data.isplayer);
                break;
            case CardType2.n_continuous:
                data.continuousnum = data.isplayer ? pContinuous : eContinuous;
                int extrahit = 0;
                //目前判断超过1就生效+damage2
                if (data.isplayer && pContinuous >= 1) extrahit = data._card.damage2;
                if (!data.isplayer && eContinuous >= 1) extrahit = data._card.damage2;
                data.hitnum = data._card.damage1 + extrahit;
                continuousAdd(data.isplayer);
                break;
            case CardType2.n_thump:
                if (data.isplayer && willTakeenemyque.Count<=0)
                    data.hitnum = data._card.damage1 + data._card.damage2;
                if (!data.isplayer && willTakeplayerque.Count <= 0)
                    data.hitnum = data._card.damage1 + data._card.damage2;
                continuousAdd(data.isplayer);
                break;
            case CardType2.n_recover:
                data.recovernum = data._card.damage1;
                continuousShut(data.isplayer);
                break;
            case CardType2.n_defence:
                data.defnum = data._card.damage1;
                continuousShut(data.isplayer);
                break;
            case CardType2.n_counter:
                if (data.isplayer) iscounterE = true;
                else iscounterP = true;
                continuousShut(data.isplayer);
                break;
            case CardType2.n_deal:
                data.dealnum = data._card.damage1;
                break;
            case CardType2.e_deplete:
                data.hitnum = data._card.damage1;
                data.hitselfnum = data._card.damage2;
                continuousAdd(data.isplayer);
                break;
            case CardType2.e_gift:
                break;
            case CardType2.e_addition:
                break;
            case CardType2.e_defend:
                if (data.isplayer) isdefendP = true;
                else isdefendE = true;
                conditionTypeCalculate1(data);
                break;
            case CardType2.e_power:
                conditionTypeCalculate1(data);
                if ((ispowerP && data.isplayer) || (ispowerE && !data.isplayer))
                    conditionTypeCalculate2(data);
                break;
            case CardType2.e_decounter:
                conditionTypeCalculate1(data);
                if (data.isdecounter)
                    conditionTypeCalculate2(data);
                break;
        }
        //屏障 特殊结算
        if (data.hitnum > 0)
        {
            if(data.isplayer == isdefendE)
            {
                isdefendE = false;
                data.hitnum = 0;
                data.isdefend = true;
            }
            if (!data.isplayer == isdefendP)
            {
                isdefendP = false;
                data.hitnum = 0;
                data.isdefend = true;
            }
        }
        playCardNext(data);
    }

    //条件 check
    private void conditionTypeCalculate1(RoundData data)
    {
        switch (data._card.conditionType)
        {
            case CardType2.n_hit:
                data.hitnum = data._card.damage1;
                if (data._card.damage1 > 0)
                    continuousAdd(data.isplayer);
                break;
            case CardType2.n_deal:
                data.dealnum = data._card.damage1;
                continuousShut(data.isplayer);
                break;
            case CardType2.n_defence:
                data.defnum = data._card.damage1;
                continuousShut(data.isplayer);
                break;
            case CardType2.n_recover:
                data.recovernum = data._card.damage1;
                continuousShut(data.isplayer);
                break;
        }
    }
    private void conditionTypeCalculate2(RoundData data)
    {
        switch (data._card.conditionType2)
        {
            case CardType2.n_hit:
                data.hitnum += data._card.damage2;
                break;
            case CardType2.n_deal:
                data.dealnum += data._card.damage2;
                break;
            case CardType2.n_defence:
                data.defnum += data._card.damage2;
                break;
            case CardType2.n_recover:
                data.recovernum += data._card.damage2;
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

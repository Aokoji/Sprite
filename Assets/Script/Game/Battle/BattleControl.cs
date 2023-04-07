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
        ui.initData(enemy.takeDefaultCardsID);
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

    bool iswin;

    public void StartRound()
    {
        //回合开始 发牌
        ui.dealCard(4);
    }


    //出牌结束 开始统计回合
    private void settleRoundAction(List<CardEntity> datas)
    {
        var endata = enemyround();
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
                if (rounddata._card.type2 != CardType2.n_preempt || playerque.Count <= 0)
                    isplayerround = true;
                if (playerque.Count <= 0) isplayerround = true;
                willTakeenemyque.Enqueue(rounddata);
            }
        }
        round = 0;
        pContinuous = 0;
        eContinuous = 0;
        iscounterP = false;
        iscounterE = false;
        if (willTake.Count > 0)
            roundNext();
        else
            Debug.LogError("无人出牌 处理一下逻辑问题。");
    }
    private List<CardEntity> enemyround()
    {   //+++ai逻辑需要补全
        List<CardEntity> result = new List<CardEntity>();
        return result;
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
            data.isCounter = true;
            continuousShut(data.isplayer);
            playCardNext(data);
            return;
        }
        //计算伤害
        data.hitnum = data._card.damage1;
        switch (data._card.type2)
        {
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
                data.recovernum = data._card.damage1;
                continuousShut(data.isplayer);
                break;
            case CardType2.n_counter:
                if (data.isplayer) iscounterE = true;
                else iscounterP = true;
                continuousShut(data.isplayer);
                break;
        }
        playCardNext(data);
    }

    private void playCardNext(RoundData data)
    {
        //结算这回合的 ***数据***
        if (data.isCounter)
        {
            ui.playThisCard(data, player, enemy);
            return;
        }
        if (data.isplayer)
        {
            if (enemy.hp_cur <= data.hitnum)
                enemy.hp_cur = 0;
            else
                enemy.hp_cur -= data.hitnum;
            if (data.recovernum > 0)
            {
                if (player.hp_cur + data.recovernum > player.hp_max)
                {
                    data.recovernum = player.hp_max - player.hp_cur;
                    player.hp_cur = player.hp_max;
                }
                else
                    player.hp_cur += data.recovernum;
            }
        }
        else
        {
            if (player.hp_cur <= data.hitnum)
                player.hp_cur = 0;
            else
                player.hp_cur -= data.hitnum;
            if (data.recovernum > 0)
            {
                if (enemy.hp_cur + data.recovernum > enemy.hp_max)
                {
                    data.recovernum = enemy.hp_max - enemy.hp_cur;
                    enemy.hp_cur = enemy.hp_max;
                }
                else
                    enemy.hp_cur += data.recovernum;
            }
        }
        //播放这张的效果
        ui.playThisCard(data, player, enemy);
    }
    //单次回合结束
    private void endRoundSettle()
    {
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

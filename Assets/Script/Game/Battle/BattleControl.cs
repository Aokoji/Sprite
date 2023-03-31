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
        //ui.initData();
        player = PlayerManager.Instance.currentSprite;
        ui.refreshPlayerData(player);
    }
    #endregion

    private PlayerData player;
    private PlayerData enemy;
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
        Queue<CardData> playerque = new Queue<CardData>();
        Queue<CardData> enemyque = new Queue<CardData>();
        datas.ForEach(item=>{ playerque.Enqueue(item._data); });
        endata.ForEach(item=>{ enemyque.Enqueue(item._data); });
        int countround = playerque.Count+enemyque.Count;
        bool isplayerround=true;
        for (int i = 0; i < countround; i++)
        {
            willTake.Enqueue(isplayerround);
            rounddata = new RoundData();
            if (isplayerround)
            {
                rounddata._card = playerque.Dequeue();
                rounddata.isplayer = true;
                if (rounddata._card.type2 != CardType2.n_preempt || playerque.Count <= 0)
                    isplayerround = false;
                willTakeplayerque.Enqueue(rounddata);
            }
            else
            {
                rounddata._card = enemyque.Dequeue();
                rounddata.isplayer = false;
                if (rounddata._card.type2 != CardType2.n_preempt || playerque.Count <= 0)
                    isplayerround = true;
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
    private void roundNext()
    {
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
        if((data.isplayer && iscounterP)|| (!data.isplayer && iscounterE))
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

    }

    private void endRoundSettle()
    {
        //伤害检测还没算
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
    #endregion

    #region  卡牌操作判断

    #endregion
}

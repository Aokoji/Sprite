using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using customEvent;

public class BattlePanel : PanelBase
{
    //创建ui
    public Transform createCardPos; //创建卡牌点
    public Transform createEnemyCardPos; 
    public Transform createCardPosIn; //创建卡牌点
    public Transform showCardPos;
    public Transform showCardPos2;

    public GameObject[] takeCardPos;     //四个点  或3-5
    public GameObject[] takeEnemyPos;     //四个点  或3-5
    public GameObject[] handCardPos;     //六个点
    public GameObject[] enemyCardPos;     //六个点
    public Transform cardParent;

    //--------------交互按钮--------------------
    public Button endRoundBtn; //回合结束
    public Button dealCardBtn;      //主动抽牌
    //--------------显示面板--------------------
    public Image spriteIcon;
    public Text health;
    public RectTransform healthimg;
    public Text defence;
    public Text manatext;
    public GameObject[] manaList;
    public GameObject manaExtra;

    public Text enemyhealth;
    public RectTransform enemyhealthimg;
    public Text enemydefence;
    public GameObject enemyExtra;

    private Queue<int> playerque;
    private Queue<int> enemyque;

    private List<CardEntity> takeCardlist = new List<CardEntity>(); //提交出牌
    private List<CardEntity> takeEnemylist = new List<CardEntity>(); //提交出牌
    private List<CardEntity> handCardlist = new List<CardEntity>();
    private List<CardEntity> handEnemylist = new List<CardEntity>();
    private Queue<CardEntity> discardCard = new Queue<CardEntity>();

    private Queue<Action> rankAction = new Queue<Action>();
    SpriteData player;
    SpriteData enemy;
    bool dealbtnAllow;
    private const float healthconstWidth = 100;
    string CARDPATH = "Assets/ui/battle/card/";
    enum rank
    {
        none,
        dealcard,
        takecard,
        roundCalcu,
        showcard,
    }
    private rank currank;

    public void initData(SpriteData enemy_data,SpriteData player_data)
    {
        player = player_data;
        enemy = enemy_data;
        getPlayerNewCardQue();
        getEnemyNewCardQue();
        refreshPlayerData();
        refreshEnemyData();
        refreshMana();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        endRoundBtn.onClick.AddListener(endroundClick);
        dealCardBtn.onClick.AddListener(dealcardClick);
    }
    private void getPlayerNewCardQue()
    {
        List<int> list;
        if (PlayerManager.Instance.getPlayerCards().Count > 0)
        {
            //自定义牌组
            list = new List<int>(PlayerManager.Instance.getPlayerCards());
            if (list.Count < 20)
                for (int i = list.Count; i < 20; i++)
                    list.Add(1);
        }
        else
        {
            //默认牌组
            var cards = Config_t_DefaultCardGroup.getOne(PlayerManager.Instance.cursprite.takeDefaultCardsID);
            string[] ids = cards.cardlist.Split('|');
            list = new List<int>();
            for (int i = 0; i < ids.Length; i++)
                list.Add(int.Parse(ids[i]));
        }
        playerque = CardCalculate.getRandomList(list);
    }
    private void getEnemyNewCardQue()
    {
        var cards = Config_t_DefaultCardGroup.getOne(enemy.takeDefaultCardsID);
        string[] ids = cards.cardlist.Split('|');
        List<int> list = new List<int>();
        for (int i = 0; i < ids.Length; i++)
            list.Add(int.Parse(ids[i]));
        enemyque = CardCalculate.getRandomList(list);
    }
    #region     阶段控制
    //开始阶段
    public void startGame()
    {
        currank = rank.dealcard;
        dealbtnAllow = true;
        addAction(() =>
        {
            initCardEnemy(4);
            dealCard(4);
        });
        playerNextQue();
    }
    private void addAction(Action action)
    {
        rankAction.Enqueue(action);
    }
    private void playerNextQue()
    {
        if (rankAction.Count <= 0)
        {
            nextRank();
            return;
        }
        rankAction.Dequeue().Invoke();
    }
    private void nextRank()
    {
        switch (currank)
        {
            case rank.dealcard:
                currank = rank.showcard;
                //回合开始
                PanelManager.Instance.showTips1("回合开始");
                break;
            case rank.showcard:
                currank = rank.roundCalcu;
                PanelManager.Instance.panelLock();
                EventAction.Instance.TriggerAction(eventType.roundEnd_C, takeCardlist);
                break;
            case rank.roundCalcu:
                currank = rank.takecard;
                EventAction.Instance.TriggerAction(eventType.playRoundNext);
                break;
            case rank.takecard:
                currank = rank.dealcard;
                refreshMana();
                addAction(() => { dealCard(1); });
                dealEnemyCard(1);
                break;
        }
    }
    #endregion
    public void initCardEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var data = Config_t_DataCard.getOne(enemyque.Dequeue());
            var item = newcard(data, true);
            item.clickAllow = false;
            handEnemylist.Add(item);
            item.transform.position = enemyCardPos[handEnemylist.Count - 1].transform.position;
            item.transform.eulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
        }
    }

    int finishNum;

    public void dealCard(int num)
    {
        //屏蔽点击
        PanelManager.Instance.panelLock();
        StartCoroutine(rundeal(num));
        //抽牌结束开放
    }
    public void dealEnemyCard(int num)
    {
        StartCoroutine(runEnemydeal(num));
    }
    IEnumerator rundeal(int num)
    {
        finishNum = 0;
        for(int i = 0; i < num; i++)
        {
            //判断没牌
            var data = Config_t_DataCard.getOne(playerque.Count <= 0? ConfigConst.dealcard_useUp:playerque.Dequeue());
            var item = newcard(data);
            item.transform.position = createCardPos.position;
            item.transform.eulerAngles = createCardPosIn.eulerAngles;
            item.transform.localScale = Vector3.one / 2;
            item.gameObject.SetActive(true);
            if (handCardlist.Count == ConfigConst.maxCardHand)
                tearCardAnim(item);
            else
            {
                handCardlist.Add(item);
                dealCardAnim(item,handCardlist.Count);
            }
            yield return new WaitForSeconds(ConfigConst.cardwait);
        }
        while (finishNum < num)
            yield return null;
        PanelManager.Instance.panelUnlock();
        playerNextQue();
    }
    IEnumerator runEnemydeal(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var data = Config_t_DataCard.getOne(enemyque.Count <= 0 ? ConfigConst.dealcard_useUp : enemyque.Dequeue());
            var item = newcard(data, true);
            item.transform.position = createEnemyCardPos.position;
            item.transform.eulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
            if (handEnemylist.Count == ConfigConst.maxCardHand)
                tearCardAnim(item);
            else
            {
                handEnemylist.Add(item);
                dealCardEnemyAnim(item, handEnemylist.Count);
            }
            yield return new WaitForSeconds(0.5f);
        }
        playerNextQue();
    }

    public List<CardEntity> getEnemyround(SpriteData data)
    {
        takeEnemylist = EnemyCalculate.calculateEnemyAction(handEnemylist, data);
        return takeEnemylist;
    }
    //整理卡槽
    private void refreshCard()
    {
        //整理卡槽
        for(int i = 0; i < handCardlist.Count; i++)
        {
            if (handCardlist[i].transform.position != handCardPos[i].transform.position)
            {
                int index = i;
                handCardlist[i].clickAllow = false;
                RunSingel.Instance.moveToAll(handCardlist[i].gameObject, handCardPos[i].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero,()=> { handCardlist[index].clickAllow = true; });
            }
        }
    }
    private void refreshEnemyCard()
    {
        //整理卡槽
        for (int i = 0; i < handEnemylist.Count; i++)
        {
            if (handEnemylist[i].transform.position != enemyCardPos[i].transform.position)
            {
                int index = i;
                RunSingel.Instance.moveToAll(handEnemylist[i].gameObject, enemyCardPos[i].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero);
            }
        }
    }
    private void refreshTakeCard()
    {
        //整理卡槽
        for (int i = 0; i < takeCardlist.Count; i++)
        {
            if (takeCardlist[i].transform.position != takeCardPos[i].transform.position)
            {
                int index = i;
                takeCardlist[i].clickAllow = false;
                RunSingel.Instance.moveToAll(takeCardlist[i].gameObject, takeCardPos[i].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero, () => { takeCardlist[index].clickAllow = true; });
            }
        }
    }
    private void refreshEnemyTakeCard()
    {
        for (int i = 0; i < takeEnemylist.Count; i++)
        {
            if (takeEnemylist[i].transform.position != takeEnemyPos[i].transform.position)
            {
                int index = i;
                RunSingel.Instance.moveToAll(takeEnemylist[i].gameObject, takeEnemyPos[i].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_refshMove, Vector3.one, Vector3.zero);
            }
        }
    }

    private void chooseCard(CardEntity card)
    {
        if (!card.isStaying && takeCardlist.Count >= ConfigConst.maxCardTake) return;//加个提示
        if (!card.isStaying && card._data.cost > player.cost_cur) return;
        card.clickAllow = false;
        if (card.isdealcreate)
        {
            takeCardlist.Remove(card);
            card.playJustHideAnim(() => {
                player.cost_cur += card._data.cost;
                refreshMana();
                card.gameObject.SetActive(false);
                discardCard.Enqueue(card);
                refreshTakeCard();
            });
            return;
        }
        if (card.isStaying)
        {
            //回卡槽
            player.cost_cur += card._data.cost;
            refreshMana();
            card.isStaying = false;
            takeCardlist.Remove(card);
            handCardlist.Add(card);
            dealCardToHand(card);
            refreshTakeCard();
        }
        else
        {
            //飞上去
            player.cost_cur -= card._data.cost;
            refreshMana();
            card.isStaying = true;
            handCardlist.Remove(card);
            takeCardlist.Add(card);
            dealCardToTake(card);
        }
        refreshCard();
    }
    
    private CardEntity newcard(t_DataCard data,bool isback=false)
    {
        CardEntity item;
        if (discardCard.Count > 0)
        {
            item = discardCard.Dequeue();
            item.transform.SetParent(cardParent);
        }
        else
            item = PanelManager.Instance.LoadUI(E_UIPrefab.cardHand, CARDPATH, cardParent).GetComponent<CardEntity>();
        item.isback = isback;
        item.initData(data);
        item.onChoose = chooseCard;
        return item;
    }
    private void dealCardAnim(CardEntity card,int topos)
    {
        RunSingel.Instance.moveToAll(card.gameObject,showCardPos.position,MoveType.moveAll_FTS, ConfigConst.cardtime_dealtoshow, Vector3.one, Vector3.zero, ()=> {
            RunSingel.Instance.moveToBezier(card.gameObject, showCardPos2.position,Vector3.Lerp(showCardPos.position,showCardPos2.position,0.5f)+Vector3.up* (showCardPos2.position.y-showCardPos.position.y)/2, ConfigConst.cardtime_showstay,()=> {
                RunSingel.Instance.moveToAll(card.gameObject, handCardPos[topos - 1].transform.position, MoveType.moveAll_STF, ConfigConst.cardtime_showtohand, Vector3.one, Vector3.zero,()=> { finishNum++; });
            });
        });
    }
    private void dealCardEnemyAnim(CardEntity card, int topos)
    {
        RunSingel.Instance.moveToAll(card.gameObject, enemyCardPos[topos - 1].transform.position, MoveType.moveAll_FTS, ConfigConst.cardtime_showtohand, Vector3.one, Vector3.zero);
    }
    private void dealCardToHand(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, handCardPos[handCardlist.Count - 1].transform.position, ConfigConst.cardtime_takeOnOff, () => {obj.clickAllow = true; });
    }
    private void dealCardToTake(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, takeCardPos[takeCardlist.Count - 1].transform.position, ConfigConst.cardtime_takeOnOff, () => {obj.clickAllow = true; });
    }
    private void tearCardAnim(CardEntity obj)
    {
        //撕卡
        RunSingel.Instance.moveToAll(obj.gameObject, showCardPos.position, MoveType.moveAll_FTS, ConfigConst.cardtime_dealtoshow, Vector3.one, Vector3.zero, () => {
            RunSingel.Instance.moveToBezier(obj.gameObject, showCardPos2.position, Vector3.Lerp(showCardPos.position, showCardPos2.position, 0.5f) + Vector3.up * (showCardPos2.position.y - showCardPos.position.y) / 2, ConfigConst.cardtime_showstay, () => {
                //撕毁动画
                obj.gameObject.SetActive(false);
                discardCard.Enqueue(obj);
                finishNum++;
            });
        });
    }
    private void dealcardClick()
    {
        if (player.cost_cur < 2) return;
        if (!dealbtnAllow) return;
        dealbtnAllow = false;
        player.cost_cur -= 2;
        refreshMana();
        var item = newcard(Config_t_DataCard.getOne(ConfigConst.dealcard_constID));
        takeCardlist.Add(item);
        item.clickAllow = false;
        item.transform.position = takeCardPos[takeCardlist.Count - 1].transform.position;
        item.transform.eulerAngles = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.isdealcreate = true;
        item.isStaying = true;
        item.gameObject.SetActive(true);
        item.playJustShowAnim(()=> { item.clickAllow = true; dealbtnAllow = true; });
    }
    private void endroundClick()
    {
        playerNextQue();
    }

    public void refreshPlayerData()
    {
        health.text = "health:"+player.hp_cur+"/"+player.hp_max;
        defence.text = player.def_cur.ToString();
        healthimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)player.hp_cur / player.hp_max * healthconstWidth);
    }
    public void refreshEnemyData()
    {
        enemyhealth.text = "health:" + enemy.hp_cur + "/" + enemy.hp_max;
        enemydefence.text = enemy.def_cur.ToString();
        enemyhealthimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)enemy.hp_cur / enemy.hp_max * healthconstWidth);
        manaExtra.SetActive(enemy.cost_max == 4);
    }
    public void refreshMana()
    {
        manatext.text = "mana:" + player.cost_cur + "/" + player.cost_max;
        for(int i = 0; i < manaList.Length; i++)
        {
            manaList[i].SetActive(i < player.cost_cur);
        }
        if (player.cost_max < 4) manaExtra.SetActive(false);
    }
    //腾一下展示桌面  准备回合生效
    public void playRoundWillShow()
    {
        //小动画
        refreshEnemyTakeCard();
        refreshEnemyCard();
        RunSingel.Instance.laterDo(0.5f, playerNextQue);
    }
    public void playThisCard(RoundData dataround)
    {
        Debug.Log("playcard====" + dataround._card.sname);
        float effectTime = 1.25f;
        bool isplayer = dataround.isplayer;
        //播放卡
        addAction(() =>
        {
            //置顶
            dataround.entity.transform.SetAsLastSibling();
            if (!isplayer)
                dataround.entity.turnCard();
            if (dataround.isCounter)
            {
                Debug.Log("counter");
                dataround.entity.playCounterAnim(() => {
                    RunSingel.Instance.laterDo(0.5f, () => { cardAlign(dataround); });
                    
                });
            }
            else
            {
                RunSingel.Instance.moveToAll(dataround.entity.gameObject, dataround.entity.transform.position + (isplayer ? Vector3.up : Vector3.down), MoveType.moveAll_FTS, effectTime, Vector3.one * 1.5f, Vector3.zero, () => {
                    //消失动画
                    Debug.Log("docallback");
                    dataround.entity.gameObject.SetActive(false);
                    cardAlign(dataround);
                });
            }
        });
        if (!dataround.isCounter)
        {
            //先抽牌
            if (dataround.dealnum > 0)
            {
                if (isplayer) addAction(() => { dealCard(dataround.dealnum); });
                else addAction(() => { dealEnemyCard(dataround.dealnum); });
            }
            if (dataround._card.type2 == CardType2.n_counter)
            {
                addAction(() => {
                    RunSingel.Instance.laterDo(0.5f, playerNextQue);
                });
            }
            if (dataround._card.type2 >= CardType2.e_decounter && dataround._card.type2 <= CardType2.e_decounter_recover)
            {
                addAction(() => {
                    RunSingel.Instance.laterDo(0.5f, playerNextQue);
                });
            }
            //攻击
            if (dataround.hitnum > 0)
            {
                addAction(() => {
                    ParticleManager.Instance.playEffect(E_Particle.particle_boom, isplayer ? enemyhealth.transform.position : health.transform.position);
                    refreshPlayerData();
                    refreshEnemyData();
                    RunSingel.Instance.laterDo(1, playerNextQue);
                });
            }
            if (dataround.defnum > 0)
            {
                addAction(() => {
                    refreshPlayerData();
                    refreshEnemyData();
                    RunSingel.Instance.laterDo(0.5f, playerNextQue);
                });
            }
        }
        //下一张
        addAction(() => { EventAction.Instance.TriggerAction(eventType.playRoundNext); });
        playerNextQue();
    }
    //展示卡对齐（回调
    void cardAlign(RoundData data)
    {
        //对齐
        discardCard.Enqueue(data.entity);
        if (data.isplayer)
        {
            takeCardlist.Remove(data.entity);
            refreshTakeCard();
        }
        else
        {
            takeEnemylist.Remove(data.entity);
            refreshEnemyTakeCard();
        }
        playerNextQue();
    }
    public void roundEndAndContinue()
    {
        playerNextQue();
    }
    public void gameSettle(bool iswin)
    {
        PanelManager.Instance.showTips1("游戏结束");
        PanelManager.Instance.panelLock();
    }
}

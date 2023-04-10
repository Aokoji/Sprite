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
    public GameObject[] handCardPos;     //六个点
    public GameObject[] enemyCardPos;     //六个点
    public Transform cardParent;

    public Transform moveBar;
    public Transform moveBar2;

    //--------------交互按钮--------------------
    public Button endRoundBtn; //回合结束
    public Button dealCardBtn;      //主动抽牌
    //--------------显示面板--------------------
    public Image spriteIcon;
    public Text manaMax;
    public Text manaCur;
    public Text healthMax;
    public Text healthCur;
    public Image healthimg;
    public Text defence;

    private Queue<int> playerque;
    private Queue<int> enemyque;

    private List<CardEntity> takeCardlist = new List<CardEntity>(); //提交出牌
    private List<CardEntity> handCardlist = new List<CardEntity>();
    private List<CardEntity> handEnemylist = new List<CardEntity>();
    private Queue<CardEntity> discardCard = new Queue<CardEntity>();

    private Vector3 barpos1;
    private Vector3 barpos2;
    public void initData(int enemyid)
    {
        barpos1 = moveBar.position;
        barpos2 = moveBar2.position;
        getPlayerNewCardQue();
        getEnemyNewCardQue(enemyid);
    }
    public override void registerEvent()
    {
        base.registerEvent();
        endRoundBtn.onClick.AddListener(endroundClick);
        dealCardBtn.onClick.AddListener(dealcardClick);
    }
    private void getPlayerNewCardQue()
    {
        List<int> list = new List<int>(PlayerManager.Instance.getPlayerCards());
        if (list.Count < 20)
            for(int i = list.Count; i < 20; i++)
                    list.Add(1);
        playerque = CardCalculate.getRandomList(list);
    }
    private void getEnemyNewCardQue(int enemyid)
    {
        var cards = Config_t_EnemyCardsModel.getOne(enemyid);
        string[] ids = cards.cardlist.Split('|');
        List<int> list = new List<int>();
        for (int i = 0; i < ids.Length; i++)
            list.Add(int.Parse(ids[i]));
        enemyque = CardCalculate.getRandomList(list);
    }
    public void dealCard(int num)
    {
        //屏蔽点击
        PanelManager.Instance.panelLock();
        StartCoroutine(rundeal(num));
        //抽牌结束开放
    }
    public void initCardEnemy(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var data = Config_t_DataCard.getOne(enemyque.Dequeue());
            var item = newcard(data, true);
            handEnemylist.Add(item);
            item.transform.position = enemyCardPos[handEnemylist.Count - 1].transform.position;
            item.transform.eulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
        }
    }
    
    private int maxCardHand = 5;    //+++config     手牌最大
    private int maxCardTake = 4;    //+++config     take最大
    private float cardwait = 1.25f;//+++config    抽一张牌需要等待
    float cardtime_dealtoshow = 0.5f; //
    float cardtime_showstay = 0.7f;
    float cardtime_showtohand = 0.4f; //
    float cardtime_takeOnOff = 0.4f; //都是参数
    float cardtime_refshMove = 0.25f; //

    int finishNum;
    IEnumerator rundeal(int num)
    {
        finishNum = 0;
        for(int i = 0; i < num; i++)
        {
            var data = Config_t_DataCard.getOne(playerque.Dequeue());
            var item = newcard(data);
            item.transform.position = createCardPos.position;
            item.transform.eulerAngles = createCardPosIn.eulerAngles;
            item.transform.localScale = Vector3.one / 2;
            if (handCardlist.Count == maxCardHand)
                tearCardAnim(item);
            else
            {
                handCardlist.Add(item);
                dealCardAnim(item,handCardlist.Count);
            }
            yield return new WaitForSeconds(cardwait);
        }
        while (finishNum < num)
            yield return null;
        //refreshCard();      //抽牌防止误触还没做
        PanelManager.Instance.panelUnlock();
    }
    IEnumerator runEnemydeal(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var data = Config_t_DataCard.getOne(enemyque.Dequeue());
            var item = newcard(data, true);
            item.transform.position = createEnemyCardPos.position;
            item.transform.eulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            if (handEnemylist.Count == maxCardHand)
                tearCardAnim(item);
            else
            {
                handEnemylist.Add(item);
                dealCardEnemyAnim(item, handEnemylist.Count);
            }
            yield return new WaitForSeconds(cardwait);
        }
        //refreshCard();      //抽牌防止误触还没做
    }
    public List<CardEntity> getEnemyround(SpriteData data)
    {
        return EnemyCalculate.calculateEnemyAction(handEnemylist, data);
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
                RunSingel.Instance.moveToAll(handCardlist[i].gameObject, handCardPos[i].transform.position, MoveType.moveAll_FTS, cardtime_refshMove, Vector3.one, Vector3.zero,()=> { handCardlist[index].clickAllow = true; });
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
                RunSingel.Instance.moveToAll(takeCardlist[i].gameObject, takeCardPos[i].transform.position, MoveType.moveAll_FTS, cardtime_refshMove, Vector3.one, Vector3.zero, () => { takeCardlist[index].clickAllow = true; });
            }
        }
    }

    private void chooseCard(CardEntity card)
    {
        if (!card.isStaying && takeCardlist.Count >= maxCardTake) return;//加个提示
        card.clickAllow = false;
        if (card.isStaying)
        {
            //回卡槽
            card.isStaying = false;
            takeCardlist.Remove(card);
            handCardlist.Add(card);
            dealCardToHand(card);
            refreshTakeCard();
        }
        else
        {
            //飞上去
            card.isStaying = true;
            handCardlist.Remove(card);
            takeCardlist.Add(card);
            dealCardToTake(card);
        }
        refreshCard();
    }
    string CARDPATH= "Assets/ui/battle/card/";
    
    private CardEntity newcard(t_DataCard data,bool isback=false)
    {
        CardEntity item;
        if (discardCard.Count > 0)
            item = discardCard.Dequeue();
        else
            item = PanelManager.Instance.LoadUI(E_UIPrefab.cardHand, CARDPATH, cardParent).GetComponent<CardEntity>();
        item.isback = isback;
        item.initData(data);
        item.onChoose = chooseCard;
        return item;
    }
    private void dealCardAnim(CardEntity card,int topos)
    {
        RunSingel.Instance.moveToAll(card.gameObject,showCardPos.position,MoveType.moveAll_FTS, cardtime_dealtoshow, Vector3.one, Vector3.zero, ()=> {
            RunSingel.Instance.moveToBezier(card.gameObject, showCardPos2.position,Vector3.Lerp(showCardPos.position,showCardPos2.position,0.5f)+Vector3.up* (showCardPos2.position.y-showCardPos.position.y)/2, cardtime_showstay,()=> {
                RunSingel.Instance.moveToAll(card.gameObject, handCardPos[topos - 1].transform.position, MoveType.moveAll_STF, cardtime_showtohand, Vector3.one, Vector3.zero,()=> { finishNum++; });
            });
        });
    }
    private void dealCardEnemyAnim(CardEntity card, int topos)
    {
        RunSingel.Instance.moveToAll(card.gameObject, enemyCardPos[topos - 1].transform.position, MoveType.moveAll_FTS, cardtime_dealtoshow, Vector3.one, Vector3.zero);
    }
    private void dealCardToHand(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, handCardPos[handCardlist.Count - 1].transform.position, cardtime_takeOnOff, () => {obj.clickAllow = true; });
    }
    private void dealCardToTake(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, takeCardPos[takeCardlist.Count - 1].transform.position, cardtime_takeOnOff, () => {obj.clickAllow = true; });
    }
    private void tearCardAnim(CardEntity obj)
    {
        //撕卡
    }
    private void dealcardClick()
    {

    }
    private void endroundClick()
    {
        PanelManager.Instance.panelLock();
        EventAction.Instance.TriggerAction(eventType.roundEnd_C, takeCardlist);
    }

    public void refreshPlayerData(SpriteData player)
    {
        healthCur.text = player.hp_cur.ToString();
        healthMax.text = player.hp_max.ToString();
        defence.text = player.def_cur.ToString();
    }
    public void refreshEnemyData(SpriteData player)
    {
        healthCur.text = player.hp_cur.ToString();
        healthMax.text = player.hp_max.ToString();
        defence.text = player.def_cur.ToString();
    }
    //腾一下展示桌面  准备回合生效
    public void playRoundWillShow(Action callback)
    {
        RunSingel.Instance.moveToAll(moveBar.gameObject, barpos1 + Vector3.down, MoveType.moveAll_FTS, 1, Vector3.one, Vector3.zero);
        RunSingel.Instance.moveToAll(moveBar2.gameObject, barpos2 + Vector3.down, MoveType.moveAll_FTS, 1, Vector3.one, Vector3.zero,callback);
    }
    public void playThisCard(RoundData dataround, SpriteData p, SpriteData e)
    {
        Debug.Log("takeCardName====     " + dataround._card.sname);
        //播放卡
        float effectTime = 1.25f;
        if (dataround.isplayer)
        {
            RunSingel.Instance.moveToAll(dataround.entity.gameObject, dataround.entity.transform.position + Vector3.up, MoveType.moveAll_FTS, effectTime, Vector3.one * 1.5f, Vector3.zero,()=> {
                //数据展示  根据rounddata的type表现
                RunSingel.Instance.laterDo(1.5f, () =>
                {
                    EventAction.Instance.TriggerAction(eventType.playRoundNext);
                    //refreshPlayerData(p);
                    //refreshEnemyData(e);
                });
                //消失动画
                dataround.entity.gameObject.SetActive(false);
                //对齐
                takeCardlist.Remove(dataround.entity);
                discardCard.Enqueue(dataround.entity);
                refreshTakeCard();
            });
        }
        else
        {
            //enemy需要翻卡
            dataround.entity.turnCard();
            RunSingel.Instance.moveToAll(dataround.entity.gameObject, dataround.entity.transform.position + Vector3.down, MoveType.moveAll_FTS, effectTime, Vector3.one * 1.5f, Vector3.zero, () => {
                //数据展示  根据rounddata的type表现
                RunSingel.Instance.laterDo(1.5f, () =>
                {
                    EventAction.Instance.TriggerAction(eventType.playRoundNext);
                    //refreshPlayerData(p);
                    //refreshEnemyData(e);
                });
                //消失动画
                dataround.entity.gameObject.SetActive(false);
                //对齐
                discardCard.Enqueue(dataround.entity);
                refreshTakeCard();
            });
        }
    }
    public void roundEndAndContinue()
    {
        PanelManager.Instance.panelUnlock();
    }
    public void gameSettle(bool iswin)
    {
        Debug.Log("gamesettle-----");
    }
}

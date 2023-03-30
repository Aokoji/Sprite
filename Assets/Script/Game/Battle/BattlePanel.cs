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
    public Transform createCardPosIn; //创建卡牌点
    public Transform showCardPos;
    public Transform showCardPos2;
    public GameObject[] takeCardPos;     //四个点  
    public GameObject[] handCardPos;     //六个点

    public Transform cardParent;

    //--------------交互按钮--------------------
    public Button endRoundBtn; //回合结束
    public Button dealCardBtn;      //主动抽牌
    //------------------

    private PlayerData player;
    private PlayerData enemy;
    private Queue<CardData> playerque;
    private Queue<CardData> enemyque;

    private List<CardEntity> takeCardlist = new List<CardEntity>(); //提交出牌
    private List<CardEntity> handCardlist = new List<CardEntity>();
    private List<CardEntity> handEnemylist = new List<CardEntity>();
    public override void init()
    {
        getPlayerNewCardQue();
        getEnemyNewCardQue();
        player = PlayerManager.Instance.currentSprite;
        initEvent();
    }
    private void getPlayerNewCardQue()
    {
        playerque = CardCalculate.getRandomList(PlayerManager.Instance.currentCardDic);
    }
    private void getEnemyNewCardQue()
    {
        enemyque = CardCalculate.getRandomList(PlayerManager.Instance.currentCardDic);//敌人管理器 +++还没做
    }
    private void initEvent()
    {
        endRoundBtn.onClick.AddListener(endroundClick);
        dealCardBtn.onClick.AddListener(dealcardClick);
    }

    public void dealCard(int num)
    {
        //屏蔽点击
        PanelManager.Instance.panelLock();
        StartCoroutine(rundeal(num));
        //抽牌结束开放
    }
    private int maxCardHand = 5;    //+++config     手牌最大
    private int maxCardTake = 4;    //+++config     take最大
    private float cardwait = 1f;//+++config    抽一张牌需要等待
    float cardtime_dealtoshow = 0.6f; //
    float cardtime_showstay = 0.75f;
    float cardtime_showtohand = 0.5f; //
    float cardtime_takeOnOff = 0.6f; //都是参数
    
    IEnumerator rundeal(int num)
    {
        for(int i = 0; i < num; i++)
        {
            var data = playerque.Dequeue();
            var item = newcard(data);
            item.clickAllow = false;
            if (handCardlist.Count == maxCardHand)
                tearCardAnim(item);
            else
            {
                handCardlist.Add(item);
                dealCard(item);
            }
            yield return new WaitForSeconds(cardwait);
        }
        refreshCard();      //抽牌防止误触还没做
        PanelManager.Instance.panelUnlock();
    }
    private void refreshCard()
    {
        //整理卡槽
    }

    private void chooseCard(CardEntity card)
    {
        if (takeCardlist.Count >= maxCardTake) return;//加个提示
        card.clickAllow = false;
        if (card.isStaying)
        {
            //回卡槽
            card.isStaying = false;
            takeCardlist.Remove(card);
            handCardlist.Add(card);
            dealCardToHand(card);
        }
        else
        {
            //飞上去
            card.isStaying = true;
            handCardlist.Remove(card);
            takeCardlist.Add(card);
            dealCardToTake(card);
        }
    }
    string CARDPATH= "Assets/ui/battle/card/";
    
    private CardEntity newcard(CardData data)
    {
        var item = PanelManager.Instance.LoadUI(E_UIPrefab.cardBase, CARDPATH, cardParent).GetComponent<CardEntity>();
        item.initData(data);
        item.onChoose = chooseCard;
        item.transform.position = createCardPos.position;
        item.transform.eulerAngles = createCardPosIn.eulerAngles;
        item.transform.localScale = Vector3.one / 2;
        return item;
    }
    private void dealCard(CardEntity card)
    {
        RunSingel.Instance.moveToAll(card.gameObject,showCardPos,MoveType.moveAll_FTS, cardtime_dealtoshow, Vector3.one, Vector3.zero, ()=> {
            RunSingel.Instance.moveToBezier(card.gameObject, showCardPos2,Vector3.Lerp(showCardPos.position,showCardPos2.position,0.5f)+Vector3.up* (showCardPos2.position.y-showCardPos.position.y)/2, cardtime_showstay,()=> {
                RunSingel.Instance.moveToAll(card.gameObject, handCardPos[handCardlist.Count - 1].transform, MoveType.moveAll_STF, cardtime_showtohand, Vector3.one, Vector3.zero);
            });
        });
    }
    private void dealCardToHand(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, handCardPos[handCardlist.Count - 1].transform, cardtime_takeOnOff, () => { obj.clickAllow = true; });
    }
    private void dealCardToTake(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, takeCardPos[takeCardlist.Count - 1].transform, cardtime_takeOnOff, () => { obj.clickAllow = true; });
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
        EventAction.Instance.TriggerAction(eventType.roundEnd_C, takeCardlist);
    }
}

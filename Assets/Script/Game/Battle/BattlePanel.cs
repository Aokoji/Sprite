using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : PanelBase
{
    //创建ui
    public Transform createCardPos; //创建卡牌点
    public GameObject showCardPos;
    public GameObject[] takeCardPos;     //四个点  
    public GameObject[] handCardPos;     //六个点

    public Transform cardParent;


    private PlayerData player;
    private PlayerData enemy;
    private Queue<CardData> playerque;
    private Queue<CardData> enemyque;

    private List<CardEntity> takeCardlist = new List<CardEntity>();
    private List<CardEntity> handCardlist = new List<CardEntity>();
    private List<CardEntity> handEnemylist = new List<CardEntity>();

    public override void init()
    {
        getPlayerNewCardQue();
        getEnemyNewCardQue();
        player = PlayerManager.Instance.currentSprite;
        takecardRelationVec= cardParent.InverseTransformPoint(createCardPos.position);
    }
    private void getPlayerNewCardQue()
    {
        playerque = CardCalculate.getRandomList(PlayerManager.Instance.currentCardDic);
    }
    private void getEnemyNewCardQue()
    {
        enemyque = CardCalculate.getRandomList(PlayerManager.Instance.currentCardDic);//敌人管理器 +++还没做
    }
    public void dealCard(int num)
    {
        //屏蔽点击
        PanelManager.Instance.panelLock();
        StartCoroutine(rundeal(num));
        //抽牌结束开放
        refreshCard();
    }
    Vector3 takecardRelationVec;    //卡牌相对位移（牌组
    private int maxCardHand = 5;    //+++config     手牌最大
    private int maxCardTake = 4;    //+++config     take最大
    private float cardwait = 1f;//+++config    抽一张牌需要等待
    float cardflytime = 1f; //抽卡飞的时间
    IEnumerator rundeal(int num)
    {
        for(int i = 0; i < num; i++)
        {
            var data = playerque.Dequeue();
            var item = newcard(data);
            if (handCardlist.Count == maxCardHand)
                tearCardAnim(item);
            else
            {
                handCardlist.Add(item);
                dealCard(item);
            }
            yield return new WaitForSeconds(cardwait);
        }
        refreshCard();
        PanelManager.Instance.panelUnlock();
    }
    private void refreshCard()
    {
        //整理卡槽
    }

    private void chooseCard(CardEntity card)
    {
        if (takeCardlist.Count >= maxCardTake) return;//加个提示
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
        item.transform.localPosition = takecardRelationVec;
        item.transform.localEulerAngles = createCardPos.localEulerAngles;
        item.transform.localScale = Vector3.one / 2;
        return item;
    }
    private void dealCard(CardEntity card)
    {
        RunSingel.Instance.moveToAll(card.gameObject,showCardPos, cardflytime,Vector3.one,new Quaternion(),()=> {
            RunSingel.Instance.moveToAll(card.gameObject, handCardPos[handCardlist.Count - 1], cardflytime, Vector3.one, new Quaternion());
        });
    }
    private void dealCardToHand(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, handCardPos[handCardlist.Count - 1], cardflytime);
    }
    private void dealCardToTake(CardEntity obj)
    {
        RunSingel.Instance.moveTo(obj.gameObject, takeCardPos[takeCardlist.Count - 1], cardflytime);
    }
    private void tearCardAnim(CardEntity obj)
    {
        //撕卡
    }
    private void chooseCardAnim()
    {

    }
}

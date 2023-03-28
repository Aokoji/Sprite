using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : PanelBase
{
    public Image img1;
    public Image img2;
    //创建ui
    public Transform createCardPos; //创建卡牌点
    public GameObject[] takeCardPos;     //四个点  
    public GameObject[] handCardPos;     //六个点

    public Transform cardParent;


    private PlayerData player;
    private PlayerData enemy;
    private Queue<CardData> playerque;
    private Queue<CardData> enemyque;

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
    private float cardwait = 1.5f;//+++config    抽一张牌需要等待
    float cardflytime = 2f; //抽卡飞的时间
    IEnumerator rundeal(int num)
    {
        for(int i = 0; i < num; i++)
        {
            var data = playerque.Dequeue();
            var item = newcard(data);
            handCardlist.Add(item);
            dealCardAnim(item);
            yield return new WaitForSeconds(cardwait);
        }
        refreshCard();
        PanelManager.Instance.panelUnlock();
    }
    private void refreshCard()
    {

    }

    private void chooseCard(CardEntity card)
    {

    }
    string CARDPATH= "Assets/ui/battle/card/";
    
    private CardEntity newcard(CardData data)
    {
        var item = PanelManager.Instance.LoadUI(E_UIPrefab.cardBase, CARDPATH, cardParent).GetComponent<CardEntity>();
        item.initData(data);
        item.onChoose = chooseCard;
        item.transform.localPosition = takecardRelationVec;
        return item;
    }
    private void dealCardAnim(CardEntity obj)
    {
        //抽卡动画
        RunSingel.Instance.moveTo(obj.gameObject, handCardPos[handCardlist.Count - 1], cardflytime,1);
    }
    private void chooseCardAnim()
    {

    }
}

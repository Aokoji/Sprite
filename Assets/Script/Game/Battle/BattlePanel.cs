using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePanel : PanelBase
{
    //创建ui
    public Transform createCardPos;
    public Transform[] takeCardPos;
    public Transform[] handCardPos;



    private PlayerData player;
    private PlayerData enemy;
    private Queue<CardData> playerque;
    private Queue<CardData> enemyque;

    private List<CardData> handCardlist = new List<CardData>();
    private List<CardData> handEnemylist = new List<CardData>();
    private List<CardEntity> handCardEntity = new List<CardEntity>();

    public void initData()
    {
        getPlayerNewCardQue();
        getEnemyNewCardQue();
        player = PlayerManager.Instance.currentSprite;
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
        for (int i = 0; i < num; i++)
        {
            var data = playerque.Dequeue();
            //dealAnim(data);
            handCardlist.Add(data);
        }
        //抽牌结束开放
        refreshCard();
    }
    private void refreshCard()
    {

    }

    private void chooseCard(CardEntity card)
    {

    }

    private void dealAnim(GameObject obj, CardData card)
    {
        //抽卡动画
        var item = PanelManager.Instance.LoadUI(E_UIPrefab.cardItem).GetComponent<CardEntity>();
        item.initData(card);
        item.onChoose = chooseCard;
        //RunSingel.Instance.moveTo()
    }
    private void chooseCardAnim()
    {

    }
}

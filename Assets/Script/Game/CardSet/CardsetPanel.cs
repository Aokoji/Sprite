﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsetPanel : PanelBase
{
    public UITool_ScrollView scroll;    //暂定 之后改为翻页     该scroll为不变动列表
    public UITool_ScrollView scrollsp;    
    public CardsetItem[] cards;
    public Button savebtn;
    public Button clearbtn;
    public Button backbtn;
    public Text spriteManaText;

    public Button normalToggle;
    public Button specialToggle;

    private List<int> cardcopy; //玩家list 的复制
    //Dictionary<int, int> cardnums;
    int justBarIndex;   //显示bar特效用
    int mana;
    int maxmana;
    bool ischanged;
    bool istoggleNormal;    //切换页
    public override void init()
    {
        //cardnums = new Dictionary<int, int>(PlayerManager.Instance.playerMakenDic);
        initData();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        savebtn.onClick.AddListener(saveCards);
        clearbtn.onClick.AddListener(clearCards);
        backbtn.onClick.AddListener(backmain);
        normalToggle.onClick.AddListener(toggleClick);
        specialToggle.onClick.AddListener(toggleClick);
    }
    private Queue<CardSetEntity> discardCard = new Queue<CardSetEntity>();
    private Dictionary<int, CardSetEntity> allcards = new Dictionary<int, CardSetEntity>();
    string CARDPATH = "ui/battle/card/";

    private void initData()
    {
        //初始化scroll 和limit  限制数据
        PanelManager.Instance.LoadingShow(true);
        ischanged = false;
        cardcopy = new List<int>(PlayerManager.Instance.getPlayerCards());
        maxmana = PlayerManager.Instance.getcursprite().spritePower;
        istoggleNormal = true;
        justBarIndex = -1;
        refreshToggleBtn();
        StartCoroutine(initScrollData());
    }
    IEnumerator initScrollData()
    {
        var carddic = PlayerManager.Instance.spriteLevelCardDic;
        var prefab = PanelManager.Instance.LoadUI(E_UIPrefab.cardShow, CARDPATH);
        scroll.initConfig(150, 200, prefab.gameObject);
        scrollsp.initConfig(150, 200, prefab.gameObject);
        var normallist = TableManager.Instance.basicList;
        foreach(var item in normallist)
        {
            var config = Config_t_DataCard.getOne(item);
            if (!carddic.ContainsKey(config.limit) || config.level > carddic[config.limit]) continue;
            var card = scroll.addItemDefault().GetComponent<CardSetEntity>();
            card.initData(item,2, chooseCard);
            allcards.Add(item, card);
        }
        if (Main.AllCardOpen)
        {
            var makenlist = TableManager.Instance.allCardDic;
            foreach (var item in makenlist)
            {
                var config = item.Value;
                if (normallist.Contains(config.id) || config.type1 == CardType1.hidden || (int)config.type1>4) continue;
                if (!carddic.ContainsKey(config.limit) || config.level > carddic[config.limit]) continue;
                //添卡
                var card = scrollsp.addItemDefault().GetComponent<CardSetEntity>();
                card.initData(item.Key, 2, chooseCard);
                if(!allcards.ContainsKey(card._data.id))
                    allcards.Add(card._data.id, card);
            }
        }
        else
        {
            var makenlist = PlayerManager.Instance.playerMakenDic;
            foreach (var item in makenlist)
            {
                var config = Config_t_DataCard.getOne(item.Key);
                if (config.level > carddic[config.limit]) continue;
                //添卡
                var card = scrollsp.addItemDefault().GetComponent<CardSetEntity>();
                card.initData(item.Key, item.Value, chooseCard);
                allcards.Add(card._data.id, card);
            }
        }
        
        /*
        foreach (var item in Config_t_DataCard._data)
        {
            if (item.Value.limitcount == 99) continue;
            if (item.Value.type1 != CardType1.take && item.Value.type1 != CardType1.untaken) continue;
            if (!carddic.ContainsKey(item.Value.limit) || item.Value.level > carddic[item.Value.limit]) continue;
            //添卡
            var card = scroll.addItemDefault().GetComponent<CardSetEntity>();
            card.initData(item.Value, GameManager.isAllCardOpen ? 2 : PlayerManager.Instance.playerMakenDic[item.Key], this);
            allcards.Add(card._data.id, card);
        }
        foreach (var item in Config_t_DataCard._data)
        {
            if (item.Value.limitcount == 99) continue;
            if (item.Value.type1 == CardType1.take || item.Value.type1 == CardType1.untaken) continue;
            if (item.Value.level > carddic[item.Value.limit]) continue;
            //添卡
            var card = scrollsp.addItemDefault().GetComponent<CardSetEntity>();
            card.initData(item.Value, GameManager.isAllCardOpen ? 2 : PlayerManager.Instance.playerMakenDic[item.Key], this);
            allcards.Add(card._data.id, card);
        }
        */
        List<int> list = new List<int>();   //短暂记录
        //数量限制计算
        for (int i = 0; i < cardcopy.Count; i++)
        {
            var data = Config_t_DataCard.getOne(cardcopy[i]);
            if (data.type1 == CardType1.condition)
                mana += data.cost;
            allcards[data.id].chooseThisCard();
        }
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].init();
            cards[i].onchoose = releaseCard;
        }
        scroll.reCalculateHeigh();
        scrollsp.reCalculateHeigh();
        yield return null;
        refreshWillList();
        refreshMana();
        PanelManager.Instance.LoadingShow(false);
    }

    //刷新卡组
    private void refreshWillList()
    {
        for (int i= 0; i < cards.Length; i++)
        {
            if (i >= cardcopy.Count)
                cards[i].setData(null);
            else
                cards[i].setData(Config_t_DataCard.getOne(cardcopy[i]));
        }
        if (justBarIndex >= 0)
        {
            ParticleManager.Instance.playEffect(E_Particle.particle_chooseCardBar, cards[justBarIndex].transform.position);
            justBarIndex = -1;
        }
    }
    private void refreshMana()
    {
        if (mana > maxmana)
            spriteManaText.color = Color.red;
        else
            spriteManaText.color = Color.white;
        spriteManaText.text = mana + "/" + maxmana;
    }
    public void chooseCard(CardSetEntity card)
    {
        if (checkCardsFull())
        {
            PanelManager.Instance.showTips3("卡组已满");
            return;
        }
        int cardid = card._data.id;
        card.chooseThisCard();
        card.showChooseParticle();
        //能进这里代表能choose
        if (cardcopy.Count >= 20) return;

        ischanged = true;
        bool addsuccess=false;
        for(int i = 0; i < cardcopy.Count; i++)
        {
            if (cardcopy[i] == cardid)
            {
                cardcopy.Insert(i, cardid);
                justBarIndex = i;
                addsuccess = true;
                break;
            }
            if (Config_t_DataCard.getOne(cardcopy[i]).cost > allcards[cardid]._data.cost)
            {
                cardcopy.Insert(i, cardid);
                justBarIndex = i;
                addsuccess = true;
                break;
            }
        }
        if (!addsuccess)
        {
            justBarIndex = cardcopy.Count;
            cardcopy.Add(cardid);
        }
        if (allcards[cardid]._data.type1 == CardType1.condition)
        {
            mana += allcards[cardid]._data.cost;
            refreshMana();
        }
        refreshWillList();
    }
    //点击弹出卡组
    private void releaseCard(int card)
    {
        ischanged = true;
        cardcopy.Remove(card);
        var data = Config_t_DataCard.getOne(card);
        if (data.type1 == CardType1.condition)
        {
            mana -= data.cost;
            refreshMana();
        }
        allcards[card].comeBackCard();
        refreshWillList();
    }
    //设置可点击
    public bool checkCardsFull()
    {
        return cardcopy.Count >= 20;
    }

    void refreshToggleBtn()
    {
        if (istoggleNormal)
        {
            normalToggle.GetComponent<CanvasGroup>().alpha = 1;
            specialToggle.GetComponent<CanvasGroup>().alpha = 0.4f;
            normalToggle.enabled = false;
            specialToggle.enabled = true;
            scroll.gameObject.SetActive(true);
            scrollsp.gameObject.SetActive(false);
        }
        else
        {
            normalToggle.GetComponent<CanvasGroup>().alpha = 0.4f;
            specialToggle.GetComponent<CanvasGroup>().alpha = 1;
            normalToggle.enabled = true;
            specialToggle.enabled = false;
            scrollsp.gameObject.SetActive(true);
            scroll.gameObject.SetActive(false);
        }
    }

    void toggleClick()
    {
        istoggleNormal = !istoggleNormal;
        refreshToggleBtn();
        if (istoggleNormal)
        {
            //刷普通界面

        }
        else
        {
            //刷制作卡
        }
    }

    private void saveCards()
    {
        if (mana > maxmana)
        {
            PanelManager.Instance.showTips3("卡牌能量超过上限");
            return;
        }
        ischanged = false;
        PlayerManager.Instance.setPlayerCards(cardcopy);
        PanelManager.Instance.showTips1("保存成功");
    }
    private void clearCards()
    {
        foreach (var i in cardcopy)
            allcards[i].comeBackCard();
        cardcopy.Clear();
        refreshWillList();
    }
    private void backmain()
    {
        if (ischanged) PanelManager.Instance.showTips2("卡组有改动还未保存，是否退出？",
            () => { PanelManager.Instance.DisposePanel(); }
             );
        else
            PanelManager.Instance.DisposePanel();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsetPanel : PanelBase
{
    public UITool_ScrollView scroll;    //暂定 之后改为翻页
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
        base.init();
        scroll.initConfig(150, 200);
        //cardnums = new Dictionary<int, int>(PlayerManager.Instance.playerMakenDic);
        initData();
        scroll.reCalculateHeigh();
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
        maxmana = PlayerManager.Instance.cursprite.spritePower;
        istoggleNormal = true;
        justBarIndex = -1;
        refreshToggleBtn();
        StartCoroutine(initScrollData());
    }
    IEnumerator initScrollData()
    {
        foreach (var item in Config_t_DataCard._data)
        {
            if (item.Value.limitcount == 99) continue;
            if (item.Value.type1 != CardType1.take && item.Value.type1 != CardType1.untaken) continue;
            //添卡
            var card = newcard(item.Value);
            allcards.Add(card._data.id, card);
            scroll.addNewItem(card.gameObject);
        }
        foreach (var item in Config_t_DataCard._data)
        {
            if (item.Value.limitcount == 99) continue;
            if (item.Value.type1 == CardType1.take || item.Value.type1 == CardType1.untaken) continue;
            //添卡
            var card = newcard(item.Value);
            allcards.Add(card._data.id, card);
            scrollsp.addNewItem(card.gameObject);
        }
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
    private CardSetEntity newcard(t_DataCard data, bool isback = false)
    {
        CardSetEntity item;
        if (discardCard.Count > 0)
        {
            item = discardCard.Dequeue();
            item.transform.SetAsLastSibling();
        }
        else
        {
            item = PanelManager.Instance.LoadUI(E_UIPrefab.cardShow, CARDPATH).GetComponent<CardSetEntity>();
        }
        item.initData(data, GameManager.isAllCardOpen ? 2 : PlayerManager.Instance.playerMakenDic[data.id], this);
        return item;
    }

    public void chooseCard(int cardid)
    {
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

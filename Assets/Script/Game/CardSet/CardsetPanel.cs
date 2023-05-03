using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsetPanel : PanelBase
{
    public UITool_ScrollView scroll;    //暂定 之后改为翻页
    public CardsetItem[] cards;
    public Button savebtn;
    public Button clearbtn;
    public Button backbtn;
    public Text spriteManaText;

    private List<int> cardcopy; //玩家list 的复制
    int mana;
    int maxmana;
    bool ischanged;
    public override void init()
    {
        base.init();
        scroll.initConfig(150, 200);
        initData();
        scroll.reCalculateHeigh();
    }
    public override void registerEvent()
    {
        base.registerEvent();
        savebtn.onClick.AddListener(saveCards);
        clearbtn.onClick.AddListener(clearCards);
        backbtn.onClick.AddListener(backmain);
    }
    private Queue<CardSetEntity> discardCard = new Queue<CardSetEntity>();
    private Dictionary<int, CardSetEntity> allcards = new Dictionary<int, CardSetEntity>();
    private Dictionary<int, int> limitedCard = new Dictionary<int, int>();
    string CARDPATH = "Assets/ui/battle/card/";
    int justAdd;

    private void initData()
    {
        //初始化scroll 和limit  限制数据
        ischanged = false;
        cardcopy = new List<int>(PlayerManager.Instance.getPlayerCards());
        maxmana = PlayerManager.Instance.cursprite.spritePower;
        initScrollData();
        refreshWillList();
        refreshMana();
    }
    private void initScrollData()
    {
        List<int> list = new List<int>();   //短暂记录
        foreach (var item in Config_t_DataCard._data)
        {
            if (item.Value.limitcount == 99) continue;
            //添卡
            var card = newcard(item.Value);
            allcards.Add(card._data.id, card);
            scroll.addNewItem(card.gameObject);
        }
        for(int i = 0; i < cardcopy.Count; i++)
        {
            var data = Config_t_DataCard.getOne(cardcopy[i]);
            if (data.type1 == CardType1.condition)
                mana += data.cost;
            if (data.limitcount == 1)
            {
                limitedCard.Add(data.id, 1);
                setOneCardOpen(data.id,false);
            }
            if (data.limitcount == 2)
            {
                if (list.Contains(data.id))
                {
                    limitedCard.Add(data.id, 1);
                    setOneCardOpen(data.id, false);
                }
                else
                    list.Add(data.id);
            }
        }
        for(int i = 0; i < cards.Length; i++)
        {
            cards[i].init();
            cards[i].onchoose = releaseCard;
        }
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
            item = PanelManager.Instance.LoadUI(E_UIPrefab.cardShow, CARDPATH).GetComponent<CardSetEntity>();
        item.initData(data);
        item.onChoose = chooseCard;
        return item;
    }

    private void chooseCard(CardSetEntity card)
    {
        int id = card._data.id;
        if (limitedCard.ContainsKey(id)) return;
        //能进这里代表能choose
        if (cardcopy.Count >= 20) return;
        ischanged = true;
        //限制卡计算
        if (card._data.limitcount == 1)
            limitedCard.Add(id, 1);
        if(card._data.limitcount == 2)
            if (cardcopy.Contains(id))
                limitedCard.Add(id, 1);
        bool addsuccess=false;
        for(int i = 0; i < cardcopy.Count; i++)
        {
            if (cardcopy[i] == id)
            {
                cardcopy.Insert(i, id);
                justAdd = i;
                addsuccess = true;
                break;
            }
            if (Config_t_DataCard.getOne(cardcopy[i]).cost > card._data.cost)
            {
                cardcopy.Insert(i, id);
                justAdd = i;
                addsuccess = true;
                break;
            }
        }
        if (!addsuccess)
        {
            justAdd = cardcopy.Count;
            cardcopy.Add(id);
        }
        if (card._data.type1 == CardType1.condition)
        {
            mana += card._data.cost;
            refreshMana();
        }
        refreshOneCard(id);
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
        if (limitedCard.ContainsKey(card))
        {
            setOneCardOpen(card, true);
            limitedCard.Remove(card);
        }
        refreshWillList();
    }
    //设置可点击
    private void refreshOneCard(int id)
    {
        allcards[id].setOpen(!limitedCard.ContainsKey(id));
    }
    private void setOneCardOpen(int id, bool isopen)
    {
        allcards[id].setOpen(isopen);
    }

    private void saveCards()
    {
        if (mana > maxmana)
        {
            PanelManager.Instance.showTips1("卡牌能量超过上限");
            return;
        }
        ischanged = false;
        PlayerManager.Instance.setPlayerCards(cardcopy);
        PanelManager.Instance.showTips1("保存成功");
    }
    private void clearCards()
    {
        cardcopy.Clear();
        foreach(var i in limitedCard)
            setOneCardOpen(i.Key, true);
        limitedCard.Clear();
        refreshWillList();
    }
    private void backmain()
    {
        if (ischanged) PanelManager.Instance.showTips2("卡组有改动还未保存，是否退出？",
            () => { PanelManager.Instance.OpenPanel(E_UIPrefab.StartPanel); }
             );
        else
            PanelManager.Instance.OpenPanel(E_UIPrefab.StartPanel);
    }
}

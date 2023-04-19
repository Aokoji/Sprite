using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsetPanel : PanelBase
{
    public UITool_ScrollView scroll;    //暂定 之后改为翻页
    public CardsetItem[] cards;

    private List<int> cardcopy; //玩家list 的复制
    public override void init()
    {
        base.init();
        scroll.initConfig(150, 200);
        initData();
        scroll.reCalculateHeigh();
    }
    private Queue<CardSetEntity> discardCard = new Queue<CardSetEntity>();
    private Dictionary<int, int> limitedCard = new Dictionary<int, int>();
    string CARDPATH = "Assets/ui/battle/card/";

    private void initData()
    {
        //初始化scroll 和limit  限制数据
        initScrollData();
        cardcopy = new List<int>(PlayerManager.Instance.getPlayerCards());
        refreshWillList();
    }
    private void initScrollData()
    {
        List<int> list = new List<int>();   //短暂记录
        foreach (var item in Config_t_DataCard._data)
        {
            if (item.Value.limitcount == 1)
                limitedCard.Add(item.Key, 1);
            if (item.Value.limitcount == 2)
            {
                if (list.Contains(item.Key))
                    limitedCard.Add(item.Key, 2);
                else
                    list.Add(item.Key);
            }
            //添卡
            scroll.addNewItem(newcard(item.Value).gameObject);
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
        //能进这里代表能choose
        //限制卡计算
        if (card._data.limitcount == 1)
            limitedCard.Add(card._data.id, 1);
        if(card._data.limitcount == 2)
            if (cardcopy.Contains(card._data.id))
                limitedCard.Add(card._data.id, 1);
    }
    private void releaseCard(CardSetEntity card)
    {

    }
}

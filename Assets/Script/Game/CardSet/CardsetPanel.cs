using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsetPanel : PanelBase
{
    public UITool_ScrollView scroll;    //暂定 之后改为翻页
    public CardsetItem[] cards;

    private List<int> cardcopy;
    public override void init()
    {
        base.init();
        scroll.initConfig(150, 200);
        initData();
        scroll.reCalculateHeigh();
    }
    private Queue<CardSetEntity> discardCard = new Queue<CardSetEntity>();
    string CARDPATH = "Assets/ui/battle/card/";

    private void initData()
    {
        foreach(var item in Config_t_DataCard._data)
        {
            scroll.addNewItem(newcard(item.Value).gameObject);
        }
        cardcopy = new List<int>(PlayerManager.Instance.getPlayerCards());
        refreshWillList();
    }

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

    }
}

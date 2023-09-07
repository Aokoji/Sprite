using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class EnemyCalculate:CSingel<EnemyCalculate>
{
    public static SpriteData GetEnemyData(int id)
    {
        SpriteData enemy = new SpriteData();
        enemy.Convert_Data(Config_t_ActorMessage.getOne(id));
        enemy.refreshData();
        return enemy;
    }
    internal class calcuData
    {
        public int id;
        public string sname;
        public int ishit;
        public int isrecover;
        public bool isdefence;
        public bool isdefend;
        public bool iscounter;
        public bool isdecounter;
        public bool ispreem;
        public bool isthump;
        public int isdeal;
        public bool isgift;
        public bool isaddtion;
        public bool ispower;
        public int isbroken;
        public int istodef;
        public bool isetch;
        public int cost;
        public bool ischoose;
        public float weight;
    }
    /// <summary>
    /// 旧的引用被删除，传出的参数为待处理的cardEntity
    /// </summary>
    public List<CardEntity> calculateEnemyAction(List<CardEntity> list,SpriteData data,SpriteData _pdata,int pCount,Func<CardEntity> dealaction)
    {
        List<CardEntity> result = new List<CardEntity>();
        finalList.Clear();
        cards.Clear();
        edata = data.Copy();
        pcount = pCount;
        pdata = _pdata;

        calcuData calcu;
        //StringBuilder str=new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            calcu = new calcuData();
            calcu.id = list[i]._data.id;
            calcu.cost = list[i]._data.cost;
            calcu.sname = list[i]._data.sname;
            //str.Append(list[i]._data.sname + "     ");

            calcuAllTypeSwitch(calcu, list[i]._data.type2,0);
            calcuAllTypeSwitch(calcu, list[i]._data.conditionType1, list[i]._data.damage1);
            calcuAllTypeSwitch(calcu, list[i]._data.conditionType2, list[i]._data.damage2);
            calcuAllTypeSwitch(calcu, list[i]._data.conditionType3, list[i]._data.damage3);
            cards.Add(calcu);
        }
        //Debug.Log("===" + str.ToString());
        //第一套逻辑
        //while (true){ if (!onechoose()) break;}
        //第二套逻辑
        //while (true) { if (!calcuCards()) break; }
        //第三套逻辑
        calculateCard3();

        for (int i = 0; i < finalList.Count; i++)
        {
            if (finalList[i] == data.dealCard)
            {
                result.Add(dealaction());
                continue;
            }
            for(int k = 0; k < list.Count; k++)
            {
                if (list[k]._data.id == finalList[i])
                {
                    result.Add(list[k]);
                    data.cost_cur -= list[k]._data.cost;
                    list.Remove(list[k]);
                    break;
                }
            }
        }
        weight.Clear();
        finalList.Clear();
        cards.Clear();
        //precedence.Clear();
        edata = null;
        return result;
    }

    #region 辅助计算
    void calcuAllTypeSwitch(calcuData _dat,CardType2 stype,int num)
    {
        switch (stype)
        {
            case CardType2.n_hit:_dat.ishit = num;break;
            case CardType2.n_preempt: _dat.ishit = num; _dat.ispreem = true;break;
            case CardType2.n_thump: _dat.ishit = num; _dat.isthump = true; break;
            case CardType2.n_recover: _dat.isrecover = num; break;
            case CardType2.n_defence:_dat.isdefence = true;break;
            case CardType2.n_counter:_dat.iscounter = true;break;
            case CardType2.e_defend:_dat.isdefend = true;break;
            case CardType2.n_deal:_dat.isdeal = num; break;
            case CardType2.e_deplete:_dat.ishit = num; break;
            case CardType2.e_gift:_dat.isgift = true;break;
            case CardType2.e_giftone:_dat.isgift = true;break;
            case CardType2.e_giftTwo: _dat.isgift = true;break;
            case CardType2.e_addition:_dat.isaddtion = true;break;
            case CardType2.d_power:_dat.ispower = true;break;
            case CardType2.d_decounter:_dat.isdecounter = true;break;
            case CardType2.n_broke:_dat.isbroken = num; break;
            case CardType2.s_etch:_dat.isetch = true;break;
            case CardType2.s_todefen:_dat.istodef = num; break;
        }
    }
    #endregion
    List<int> finalList = new List<int>();
    Dictionary<CardType2, int> weight = new Dictionary<CardType2, int>();
    List<calcuData> cards = new List<calcuData>();
    SpriteData edata;
    SpriteData pdata;
    int pcount;
    #region 第一套逻辑
    bool onechoose()
    {
        weight.Clear();
        var typ = checkChoose();
        if (typ == CardType2.n_enemydeal)
        {
            if(edata.cost_cur >= 2)
            {
                finalList.Add(edata.dealCard);
                edata.cost_cur -= 2;
                if (edata.cost_cur == 0) return false;
                else return true;
            }
            else
                typ = CardType2.n_hit;
        }
        for(int i = 0; i < cards.Count; i++)
        {
            var cad = cards[i];
            if (cad.ischoose) continue;
            if ((typ == CardType2.n_hit && cad.ishit > 0) ||
                (typ == CardType2.n_deal && cad.isdeal > 0) ||
                (typ == CardType2.e_defend && cad.isdefend) ||
                (typ == CardType2.n_recover && cad.isrecover > 0) ||
                (typ == CardType2.n_defence && cad.isdefence) ||
                (typ == CardType2.n_counter && cad.iscounter) ||
                (typ == CardType2.e_gift && cad.isgift))
            {
                if (cad.ispower)
                {
                    finalList.Clear();
                    finalList.Add(cad.id);
                    return false;
                }
                if (edata.cost_cur >= cad.cost)
                {
                    cad.ischoose = true;
                    edata.cost_cur -= cad.cost;
                    if(cad.ispreem|| cad.iscounter || cad.isdefend) finalList.Insert(0,cad.id);
                    else finalList.Add(cad.id);
                    if (edata.cost_cur == 0) return false;
                    return true;
                }
            }
        }
        return false;
    }
    CardType2 checkChoose()
    {
        int recovercount=0;
        int countercount=0;
        int defencecount = 0;
        int defcount=0;
        int dealcount=0;
        int giftcount=0;
        int hitcount=0;
        cards.ForEach(card =>
        {
            if (card.isrecover > 0) recovercount++;
            if (card.isdefence) defencecount++;
            if (card.iscounter) countercount++;
            if (card.isdefend) defcount++;
            if (card.isdeal > 0) dealcount++;
            if (card.isgift) giftcount++;
            if (card.ishit>0) hitcount++;
        });
        //计算权重
        if (hitcount > 0)
        {
            if (pdata.hp_cur < 10)
                weight.Add(CardType2.n_hit, 6);
            else
                weight.Add(CardType2.n_hit, 2);
        }
        if (cards.Count <= 3)
        {
            if (cards.Count <= 1)
                weight.Add(CardType2.n_deal, 20);
            else
                weight.Add(CardType2.n_deal, 8);
        }
        if (countercount > 0)
        {
            if (pcount < 3)
                if (pcount <= 1)
                    weight.Add(CardType2.n_counter, 50);
                else
                    weight.Add(CardType2.n_counter, 20);
            else
                weight.Add(CardType2.n_counter, 5);
        }
        if (recovercount > 0)
        {
            if (edata.hp_cur / edata.hp_max <= 0.8f)
                if (edata.hp_cur / edata.hp_max <= 0.25f)
                    weight.Add(CardType2.n_recover, 120);
                else
                    weight.Add(CardType2.n_recover, 3);
            else
                weight.Add(CardType2.n_recover, 1);
        }
        if (defencecount > 0)
        {
            weight.Add(CardType2.n_defence, 3);
        }
        if (giftcount > 0)
        {
            if (pcount >= 3)
                weight.Add(CardType2.e_gift, 1);
            else
                weight.Add(CardType2.e_gift, 6);
        }
        if (defcount > 0)
            weight.Add(CardType2.e_defend, 5);

        CardType2 finaltype = CardType2.n_hit;
        int max = 0;
        foreach (var item in weight)
            max += item.Value;
        max = UnityEngine.Random.Range(0, max);
        int curcount = 0;
        foreach (var item in weight)
        {
            curcount += item.Value;
            if (curcount > max)
            {
                finaltype = item.Key;
                break;
            }
        }
        if (dealcount == 0 && finaltype == CardType2.n_deal) finaltype = CardType2.n_enemydeal;
        return finaltype;
    }
    #endregion

    #region 第二套逻辑
    Dictionary<CardType2, int> precedence = new Dictionary<CardType2, int>();
    void checkPrecedence()
    {
        precedence.Clear();

        //计算优先遍历
        //hit
        precedence.Add(CardType2.n_hit, 0);
        if (pdata.hp_cur < 10)
            precedence[CardType2.n_hit] += 9;
        else if (pdata.hp_cur <= 20)
            precedence[CardType2.n_hit] += 5;
        else
            precedence[CardType2.n_hit] += 3;
        //recover && defence
        precedence.Add(CardType2.n_recover, 0);
        precedence.Add(CardType2.n_defence, 0);
        if (edata.hp_cur / edata.hp_max <= 0.6f)
            if (edata.hp_cur <=10f)
            {
                precedence[CardType2.n_recover] += 8;
                precedence[CardType2.n_hit] -= 1;
                precedence[CardType2.n_defence] += 2;
            }
            else
                precedence[CardType2.n_recover] += 3;
        else
            if (cards.Count > 3)
                precedence[CardType2.n_recover] += 2;
            else
                precedence[CardType2.n_recover] += 1;
        precedence[CardType2.n_defence] += 3;
        //gift
        if (cards.Count <=2)
            precedence.Add(CardType2.e_giftone,6);
        else if (cards.Count <= 4)
            precedence.Add(CardType2.e_giftone, 2);
        else
            precedence.Add(CardType2.e_giftone, 1);
        //counter
        if (pcount < 3)
            if (pcount <= 1)
                precedence.Add(CardType2.n_counter, 8);
            else
                precedence.Add(CardType2.n_counter, 6);
        else
            precedence.Add(CardType2.n_counter, 3);
        //deal
        if (cards.Count <= 4)
        {
            if (cards.Count <= 2)
                precedence.Add(CardType2.n_deal, 5);
            else
                precedence.Add(CardType2.n_deal, 2);
        }
        precedence.Add(CardType2.e_defend, 3);
        //最终效果  攻击，防御，恢复，抽牌，反制，补牌，屏障
    }
    bool calcuCards()
    {
        List<CardType2> manys = new List<CardType2>();
        Queue<CardType2> que = new Queue<CardType2>();
        checkPrecedence();
        //得到顺序优先级
        for (int i = 10; i >0; i--)
        {
            manys.Clear();
            foreach(var k in precedence)
            {
                if (k.Value == i)
                    manys.Add(k.Key);
            }
            if (manys.Count > 0)
            {
                if (manys.Count == 1)
                    que.Enqueue(manys[0]);
                else
                {
                    while (manys.Count > 0)
                    {
                        int id = UnityEngine.Random.Range(0, manys.Count);
                        que.Enqueue(manys[id]);
                        manys.RemoveAt(id);
                    }
                }
            }
        }
        while (que.Count > 0)
        {
            CardType2 typ = que.Dequeue();
            for (int i = 0; i < cards.Count; i++)
            {
                var cad = cards[i];
                if (cad.ischoose) continue;
                if ((typ == CardType2.n_hit && cad.ishit==0) ||
                    (typ == CardType2.n_recover && cad.isrecover==0) ||
                    (typ == CardType2.n_defence && !cad.isdefence) ||
                    (typ == CardType2.n_deal && cad.isdeal==0) ||
                    (typ == CardType2.e_defend && !cad.isdefend) ||
                    (typ == CardType2.n_counter && !cad.iscounter) ||
                    (typ == CardType2.e_giftone && !cad.isgift)
                    )
                    continue;
                if (cad.ispower)
                {
                    finalList.Clear();
                    finalList.Add(cad.id);
                    return false;
                }
                if (edata.cost_cur >= cad.cost)
                {
                    cad.ischoose = true;
                    edata.cost_cur -= cad.cost;
                    if (cad.ispreem || cad.iscounter || cad.isdefend) finalList.Insert(0, cad.id);
                    else finalList.Add(cad.id);
                    if (edata.cost_cur == 0) return false;
                    return true;
                }
            }
            //走到这说明没找到
            if(typ==CardType2.n_deal)
                if (edata.cost_cur >= 2)
                {
                    finalList.Add(edata.dealCard);
                    edata.cost_cur -= 2;
                    if (edata.cost_cur == 0) return false;
                    else return true;
                }
        }
        Debug.LogWarning("enemy有未用完能量点:  "+edata.cost_cur);
        return false;
    }
    #endregion
    #region 第三套逻辑
    //判断自身状态，判断手牌，预测下一张，对手手牌，先后手
    void calculateCard3()
    {
        List<calcuData> sortlist = new List<calcuData>();
        StringBuilder str = new StringBuilder();
        //finalList
        calcuData dealone = new calcuData();
        dealone.isdeal = 1;
        dealone.id = edata.dealCard;
        dealone.cost = 2;
        cards.Add(dealone);
        foreach(var i in cards)
        {
            //分析卡片重要程度
            float level = 0;
            //攻击优先，治疗优先，补给优先  相对程度
            float hpcalcu = 7 - edata.hp_cur * 7 / edata.hp_max;        //0-7
            float pcalcu = 8 - pdata.hp_cur * 5 / pdata.hp_max;      //3-8
            float dealcalcu = 8 - (cards.Count-1) * 8 / 6;
            if (i.ishit > 0)
                level = pcalcu + i.ishit / 3 * 2;
            if (i.isbroken > 0)
                level = pcalcu + i.isbroken / 1.5f;
            if (i.istodef > 0)
                level = pcalcu + i.istodef / 1.5f;
            if (i.isrecover>0)
            {
                if (i.ishit > 0 || i.isbroken > 0 || i.istodef > 0)
                    level = (hpcalcu + level) / 2 + i.isrecover / 4;
                else
                    level = hpcalcu + i.isrecover / 4;
            }
            if (i.isdefence)
                if (level > 0)
                    level = (level + hpcalcu * 3 / 7 + 1) / 2;
                else
                    level = hpcalcu * 3 / 7 + 1;
            if (i.isdefend)
                if (level > 0)
                    level = (level + hpcalcu * 5 / 7 + 1) / 2;
                else
                    level = hpcalcu * 5 / 7 + 1;
            if (i.isdeal > 0 || i.isgift)
                if (level > 0)
                    level = (level + dealcalcu) / 2;
                else
                    level = dealcalcu;
            if (i.isaddtion)
                if (level > 0)
                    level = (level + 6) / 2;
                else
                    level = 6;
            if (i.iscounter)
                if (level > 0)
                    level = (level + 7) / 2;
                else
                    level = 7;
            if (i.isetch)
                if (level > 0)
                    level = (level + 12) / 2;
                else
                    level = 10;
            i.weight = level;
            //排序weight
            if(sortlist.Count>0)
                for (int k = 0; k < sortlist.Count; k++)
                {
                    if(level > sortlist[k].weight)
                    {
                        sortlist.Insert(k, i);
                        break;
                    }
                }
            else
                sortlist.Add(i);
            str.Append(i.sname + level + "     ");
        }
        //出牌
        for(int i = 0; i < sortlist.Count; i++)
        {
            var dat = sortlist[i];
            if (dat.weight <= 0) continue;
            if (edata.cost_cur >= dat.cost)
            {
                //检测power
                if (dat.ispower)
                    if (finalList.Count > 0)
                        continue;
                    else
                    {
                        finalList.Add(sortlist[i].id);
                        edata.cost_cur-=dat.cost;
                        break;
                    }
                finalList.Add(sortlist[i].id);
                edata.cost_cur -= dat.cost;
            }
        }
        Debug.Log("===" + str.ToString());
    }
    #endregion
}

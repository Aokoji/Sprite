using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class EnemyCalculate:CSingel<EnemyCalculate>
{
    public static SpriteData GetEnemyData()
    {//+++
        SpriteData enemy = new SpriteData();
        enemy.Convert_Data(Config_t_ActorMessage.getOne(6));
        enemy.refreshData();
        return enemy;
    }
    internal class calcuData
    {
        public int id;
        public bool ishit;
        public bool isrecover;
        public bool isdefence;
        public bool isdefend;
        public bool iscounter;
        public bool isdecounter;
        public bool ispreem;
        public bool isthump;
        public bool isdeal;
        public bool isgift;
        public bool ispower;
        public int cost;
        public bool ischoose;
    }
    /// <summary>
    /// 旧的引用被删除，传出的参数为待处理的cardEntity
    /// </summary>
    public List<CardEntity> calculateEnemyAction(List<CardEntity> list,SpriteData data,int pHealth,int pCount,Func<CardEntity> dealaction)
    {
        List<CardEntity> result = new List<CardEntity>();
        finalList.Clear();
        cards.Clear();
        edata = data.Copy();
        pcount = pCount;
        phealth = pHealth;

        calcuData calcu;
        StringBuilder str=new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            calcu = new calcuData();
            calcu.id = list[i]._data.id;
            calcu.cost = list[i]._data.cost;
            str.Append(list[i]._data.sname + "     ");
            if (list[i]._data.type2 == CardType2.n_counter|| list[i]._data.conditionType1 == CardType2.n_counter|| list[i]._data.conditionType2 == CardType2.n_counter || list[i]._data.conditionType3 == CardType2.n_counter)
                calcu.iscounter = true;
            if (list[i]._data.type2 == CardType2.e_defend || list[i]._data.conditionType1 == CardType2.e_defend || list[i]._data.conditionType2 == CardType2.e_defend || list[i]._data.conditionType3 == CardType2.e_defend)
                calcu.isdefend = true;
            if (list[i]._data.type2 == CardType2.e_gift || list[i]._data.type2 == CardType2.e_giftone || list[i]._data.type2 == CardType2.e_addition || list[i]._data.conditionType1 == CardType2.e_giftone || list[i]._data.conditionType2 == CardType2.e_giftone || list[i]._data.conditionType3 == CardType2.e_giftone)
                calcu.isgift = true;
            if (list[i]._data.type2 == CardType2.n_deal || list[i]._data.conditionType1 == CardType2.n_deal || list[i]._data.conditionType2 == CardType2.n_deal || list[i]._data.conditionType3 == CardType2.n_deal)
                calcu.isdeal = true;
            if (list[i]._data.conditionType1 == CardType2.n_recover || list[i]._data.conditionType2 == CardType2.n_recover || list[i]._data.conditionType3 == CardType2.n_recover)
                calcu.isrecover = true;
            if(list[i]._data.conditionType1 == CardType2.n_defence || list[i]._data.conditionType2 == CardType2.n_defence || list[i]._data.conditionType3 == CardType2.n_defence)
                calcu.isdefence = true;
            if (list[i]._data.type2 == CardType2.d_decounter)
                calcu.isdecounter = true;
            if (list[i]._data.type2 == CardType2.d_power)
                calcu.ispower = true;
            if (list[i]._data.type2 == CardType2.e_deplete)
                calcu.ishit = true;
            if (list[i]._data.conditionType1 == CardType2.n_hit || list[i]._data.conditionType2 == CardType2.n_hit || list[i]._data.conditionType3 == CardType2.n_hit)
                calcu.ishit = true;
            if (list[i]._data.conditionType1 == CardType2.n_preempt || list[i]._data.conditionType2 == CardType2.n_preempt || list[i]._data.conditionType3 == CardType2.n_preempt)
                calcu.ispreem = true;
            if (list[i]._data.type2 == CardType2.n_thump || list[i]._data.conditionType1 == CardType2.n_thump || list[i]._data.conditionType2 == CardType2.n_thump || list[i]._data.conditionType3 == CardType2.n_thump)
                calcu.isthump = true;
            cards.Add(calcu);
        }
        Debug.Log("===" + str.ToString());
        //第一套逻辑
        //while (true){ if (!onechoose()) break;}
        //第二套逻辑
        while (true) { if (!calcuCards()) break; }

        for (int i = 0; i < finalList.Count; i++)
        {
            if (finalList[i] == ConfigConst.dealcard_constID)
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
        precedence.Clear();
        edata = null;
        return result;
    }
    List<int> finalList = new List<int>();
    Dictionary<CardType2, int> weight = new Dictionary<CardType2, int>();
    List<calcuData> cards = new List<calcuData>();
    SpriteData edata;
    int pcount;
    int phealth;
    #region 第一套逻辑
    bool onechoose()
    {
        weight.Clear();
        var typ = checkChoose();
        if (typ == CardType2.n_enemydeal)
        {
            if(edata.cost_cur >= 2)
            {
                finalList.Add(ConfigConst.dealcard_constID);
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
            if ((typ == CardType2.n_hit && cad.ishit) ||
                (typ == CardType2.n_deal && cad.isdeal) ||
                (typ == CardType2.e_defend && cad.isdefend) ||
                (typ == CardType2.n_recover && cad.isrecover) ||
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
            if (card.isrecover) recovercount++;
            if (card.isdefence) defencecount++;
            if (card.iscounter) countercount++;
            if (card.isdefend) defcount++;
            if (card.isdeal) dealcount++;
            if (card.isgift) giftcount++;
            if (card.ishit) hitcount++;
        });
        //计算权重
        if (hitcount > 0)
        {
            if (phealth < 10)
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
        if (phealth < 10)
            precedence[CardType2.n_hit] += 9;
        else if (phealth <= 20)
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
                if ((typ == CardType2.n_hit && !cad.ishit) ||
                    (typ == CardType2.n_recover && !cad.isrecover) ||
                    (typ == CardType2.n_defence && !cad.isdefence) ||
                    (typ == CardType2.n_deal && !cad.isdeal) ||
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
                    finalList.Add(ConfigConst.dealcard_constID);
                    edata.cost_cur -= 2;
                    if (edata.cost_cur == 0) return false;
                    else return true;
                }
        }
        Debug.LogWarning("有未用完能量点:  "+edata.cost_cur);
        return false;
    }
    #endregion
}

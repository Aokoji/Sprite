using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCalculate
{

    public static Queue<int> getRandomList(List<int> list)
    {
        System.Random r = new System.Random();
        Queue<int> result = new Queue<int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == ConfigConst.dealcard_blessed || list[i] == ConfigConst.dealcard_blessGod)
            {
                result.Enqueue(list[i]);
                list.RemoveAt(i);
                i--;
            }
        }
        while (list.Count > 0)
        {
            int index = r.Next(list.Count);
            result.Enqueue(list[index]);
            list.RemoveAt(index);
        }
        return result;
    }
    public static Queue<T> getRandomList1<T>(List<T> list)
    {
        System.Random r = new System.Random();
        Queue<T> result = new Queue<T>();
        while (list.Count > 0)
        {
            int index = r.Next(list.Count);
            result.Enqueue(list[index]);
            list.RemoveAt(index);
        }
        return result;
    }
    public static Queue<T> getRandomList<T>(Dictionary<int, T> data)
    {
        List<T> list = new List<T>();
        foreach(var item in data)
        {
            list.Add(item.Value);
        }
        return getRandomList1(list);
    }
    public static int getRandomTypeCardList(CardSelfType type)
    {
        System.Random random = new System.Random();
        return TableManager.Instance.stallCardDic[type][random.Next(TableManager.Instance.stallCardDic[type].Count)];
    }
    public static Queue<int> addOneCard(Queue<int> table,int id)
    {
        int index = Random.Range(0, table.Count);
        int count = table.Count;
        index = 1;
        Queue<int> list = new Queue<int>();
        for(int i = 0; i < count; i++)
        {
            if (i == index)
                list.Enqueue(id);
            list.Enqueue(table.Dequeue());
        }
        return list;
    }
}

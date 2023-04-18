using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCalculate
{

    public static Queue<T> getRandomList<T>(List<T> list)
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
        return getRandomList(list);
    }
    public static int getRandomTypeCardList(CardSelfType type)
    {
        int result=0;
        switch (type)
        {
            case CardSelfType.normal:
                result = TableManager.Instance.stall_normal[Random.Range(0,TableManager.Instance.stall_normal.Count)];
                break;
            case CardSelfType.fire:
                result = TableManager.Instance.stall_fire[Random.Range(0, TableManager.Instance.stall_fire.Count)];
                break;
            case CardSelfType.water:
                result = TableManager.Instance.stall_water[Random.Range(0, TableManager.Instance.stall_water.Count)];
                break;
            case CardSelfType.thunder:
                result = TableManager.Instance.stall_thunder[Random.Range(0, TableManager.Instance.stall_thunder.Count)];
                break;
            case CardSelfType.forest:
                result = TableManager.Instance.stall_forest[Random.Range(0, TableManager.Instance.stall_forest.Count)];
                break;
            case CardSelfType.arcane:
                result = TableManager.Instance.stall_arcane[Random.Range(0, TableManager.Instance.stall_arcane.Count)];
                break;
            case CardSelfType.arcane_special:
                result = TableManager.Instance.stall_arcane_special[Random.Range(0, TableManager.Instance.stall_arcane_special.Count)];
                break;
        }
        return result;
    }
}

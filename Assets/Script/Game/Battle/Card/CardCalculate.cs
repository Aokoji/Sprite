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
}

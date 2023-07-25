using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCalculate
{
    /// <summary>
    /// 计算下一等级经验
    /// </summary>
    public static int expNextCalculate(int levelnow)
    {
        //test
        return 50 + levelnow * (10 + 4 * levelnow);
    }
    public static string propertyName(int index)
    {
        switch (index)
        {
            case 1:return "生命";
            case 2:return "体力";
            default:return "";
        }
    }
    //开盒子
    public static List<ItemData> getBoxReward(int id)
    {
        List<ItemData> list = new List<ItemData>();
        Dictionary<int, int> dic = new Dictionary<int, int>();
        var item = Config_t_Consumable.getOne(id);
        string[] str = item.rewardPool.Split('|');
        System.Random random = new System.Random();
        int index = 0;
        for(int i = 0; i < item.rewardNum; i++)
        {
            index = int.Parse(str[random.Next(str.Length)]);
            if (!dic.ContainsKey(index))
                dic[index] = 0;
            dic[index]++;
        }
        foreach(var i in dic)
            list.Add(new ItemData(i.Key, i.Value));
        return list;
    }
}

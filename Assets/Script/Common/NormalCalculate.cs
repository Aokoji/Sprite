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
        System.Random random = new System.Random();
        if (item.rewardType == 1)
        {
            int index;
            int nums;
            string[] str = item.rewardPool.Split('|');
            //随机抽取奖励，数量不一定固定
            if (item.rewardNum > 0)                //固定数量奖励
                nums = item.rewardNum;
            else                //全随机
                nums = random.Next(item.takenum, item.takenum2);
            for (int i = 0; i < nums; i++)
            {
                index = int.Parse(str[random.Next(str.Length)]);
                if (!dic.ContainsKey(index))
                    dic[index] = 0;
                dic[index]++;
            }
        }
        if (item.rewardType == 2)
        {
            if (item.rewardNum > 0)          //固定奖励随机数量
                dic[item.rewardGiven] = random.Next(item.takenum, item.takenum2);
            else        //全固定
                dic[item.rewardGiven] = item.rewardNum;
        }
        foreach(var i in dic)
            list.Add(new ItemData(i.Key, i.Value));
        return list;
    }
}

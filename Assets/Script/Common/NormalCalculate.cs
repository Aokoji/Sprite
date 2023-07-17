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
}

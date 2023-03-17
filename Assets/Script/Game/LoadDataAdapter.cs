using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamedata;

public class LoadDataAdapter
{
    public static Dictionary<int, CardData> loadDataCard(string path)
    {
        var list = CSVLoad.loadCSV(path);
        Dictionary<int, CardData> result = new Dictionary<int, CardData>();
        int index = 0;
        for(int i = 1; i < list.Count; i++)
        {
            string[] str = list[i];
            index = 0;
            CardData card = new CardData();
            card.id = int.Parse(str[index++]);
            card.sname = str[index++];
            card.sDescribe = str[index++];
            card.type1 =(CardType1) int.Parse(str[index++]);
            card.limit =(CardLimitType) int.Parse(str[index++]);
            card.damage1 = int.Parse(str[index++]);
            card.damage2 = int.Parse(str[index++]);
            card.damage3 = int.Parse(str[index++]);
            card.cost = int.Parse(str[index++]);
            result.Add(card.id,card);
        }
        return result;
    }
}

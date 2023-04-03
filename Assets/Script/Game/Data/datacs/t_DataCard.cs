using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_DataCard : BaseData
{
    private string path = "t_DataCard";
    public class t_data
    {
        public int id;
        public string sname;
        public string sDescribe;
        public CardType1 type1; //卡面类型
        public CardType2 type2; //效果类型
        public CardLimitType limit;
        public int damage1;
        public int damage2;
        public int damage3;
        public int cost;
    }
    public t_DataCard ()
    {
        isloaded = false;
        praseData();
    }
    public Dictionary<int, t_data> _data;
    public bool isloaded;
    private void praseData()
    {
        _data = new Dictionary<int, t_data>();
        var list = CSVLoad.loadCSV(TABLE_PATH + path +".csv");
        int index;
        for (int i = 1; i < list.Count; i++)
        {
            string[] str = list[i];
            index = 0;
            t_data card = new t_data();
            card.id = int.Parse(str[index++]);
            card.sname = str[index++];
            card.sDescribe = str[index++];
            card.type1 = (CardType1)int.Parse(str[index++]);
            card.type2 = (CardType2)int.Parse(str[index++]);
            card.limit = (CardLimitType)int.Parse(str[index++]);
            card.damage1 = int.Parse(str[index++]);
            card.damage2 = int.Parse(str[index++]);
            card.damage3 = int.Parse(str[index++]);
            card.cost = int.Parse(str[index++]);
            _data.Add(card.id, card);
        }
        isloaded = true;
    }
}

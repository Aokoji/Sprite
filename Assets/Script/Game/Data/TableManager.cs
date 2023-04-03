using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TableManager : CSingel<TableManager>
{
    //所有卡片数据
    t_DataCard cardData;
    public bool loadsuccess;

    public void init()
    {
        loadsuccess = false;
        RunSingel.Instance.runTimer(loadData());
    }
    IEnumerator loadData()
    {
        cardData = new t_DataCard();
        while(!cardData.isloaded)
            yield return null;
        loadsuccess = true;
    }

    public t_DataCard.t_data getOneCard(int id)
    {
        if (cardData._data.ContainsKey(id))
            return cardData._data[id];
        else
            return null;
    }

    string[] loadList =
    {
        "DataTestCard",
    };
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsetItem:UIBase
{
    public Image icon;
    public Text context;
    public Text cost;

    public int id;
    public Action<int> onchoose;
    public void init()
    {
        GetComponent<Button>().onClick.AddListener(onclick);
    }
    private void onclick()
    {
        onchoose?.Invoke(id);
    }
    public void setData(t_DataCard data)
    {
        if (data == null)
        {
            id = 0;
            context.text = "（空）";
            cost.text = "";
        }
        else
        {
            id = data.id;
            context.text = data.sname;
            cost.text = data.cost.ToString();
        }
    }
}

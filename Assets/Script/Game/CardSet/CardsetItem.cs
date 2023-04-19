using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsetItem:MonoBehaviour
{
    public Image icon;
    public Text context;
    public Text cost;

    public void setData(t_DataCard data)
    {
        if (data == null)
        {

        }
        else
        {
            context.text = data.sname;
            cost.text = data.cost.ToString();
        }
    }
}

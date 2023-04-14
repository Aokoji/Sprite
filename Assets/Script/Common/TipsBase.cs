using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsBase: UIBase
{
    public Text context;

    public void setContext(string str)
    {
        context.text = str;
    }
}

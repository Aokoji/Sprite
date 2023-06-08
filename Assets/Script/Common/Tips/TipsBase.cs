using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TipsBase: UIBase
{
    public Text context;
    public Action callback;
    public Action callback2;
    public bool allowClick;

    public virtual void init(string str, Action act = null, Action act2 = null)
    {
        context.text = str;
        callback = act;
        callback2 = act2;
        allowClick = false;
        initEvent();
    }
    public virtual void setString2(string str) { }
    public virtual void init(object whatthing) { }
    public virtual void initEvent() { }
    public virtual void play() { }
}

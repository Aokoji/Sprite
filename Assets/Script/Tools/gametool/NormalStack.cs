using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NormalStack : CSingel<NormalStack>
{
    private Queue<PopTypeMachine> datalist = new Queue<PopTypeMachine>();
    Action _action;

    public void addPopEvent()
    {

    }

    public void nextpop()
    {
        if (datalist.Count > 0)
        {
            //datalist.Dequeue();
        }
    }

    public void TriggerPopEvent(Action callback=null)
    {
        _action = callback;

    }
    //清空  不常用
    public void clearPopEvent()
    {

    }
}

public class PopTypeMachine
{
    public popenum poptype;
    public string title;
    public string context;
    public object _data;
}
public enum popenum
{
    pop1,
    pop2,
    pop3,
    pop4,
    pop5,
}

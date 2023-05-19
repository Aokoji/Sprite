using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ItemData
{
    public int id;
    public int num;
    public ItemData(int tid,int tnum)
    {
        id = tid;
        num = tnum;
    }
}

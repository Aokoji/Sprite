using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class OfferData : BaseData
{
    public OfferData(int sid)
    {
        id = sid;
        finishCount = 0;
    }
    public int id;
    public int finishCount;
}

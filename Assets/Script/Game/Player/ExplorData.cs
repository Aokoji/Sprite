using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class ExplorData : MonoBehaviour
{
    public int mapType; //今日地图
    public string savetime;

    public List<OfferData> offer=new List<OfferData>();
}

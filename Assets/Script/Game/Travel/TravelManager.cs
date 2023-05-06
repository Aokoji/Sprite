using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class TravelManager : CSingel<TravelManager>
{
    TravelData _data;

    public void init()
    {
        _data = AssetManager.loadJson<TravelData>(S_SaverNames.entrust.ToString());
        if (_data == null)
        {
            _data = new TravelData();
            _data.initdata();
            saveTravel();
        }
    }
    public void saveTravel() { AssetManager.saveJson(S_SaverNames.entrust.ToString(), _data); }
    public void goTravel(int spid,int spend)
    {

    }

    void refreshTravel()
    {

    }

    void calculateTravelSpend(int spend)
    {

    }
}

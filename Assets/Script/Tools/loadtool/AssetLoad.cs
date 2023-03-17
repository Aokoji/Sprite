using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoad : CSingel<AssetLoad>
{


    public T loadUIPrefab<T>(string spath,string context) where T:Object
    {
        return AssetManager.loadAsset<T>(spath, context);
    }

}
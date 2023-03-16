using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetManager
{
    public static T loadAsset<T>(string path) where T : Object
    {
#if AB_LOAD
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        ab.LoadAsset<T>("");
        return null;
#else
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#endif
    }
}

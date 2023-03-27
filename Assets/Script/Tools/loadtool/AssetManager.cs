﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class AssetManager
{
    private static Dictionary<string, AssetBundle> abSave = new Dictionary<string, AssetBundle>();

    public static T loadAsset<T>(string spath) where T : Object
    {
#if AB_LOAD
        AssetBundle ab;
        if (!abSave.ContainsKey(spath))
            abSave.Add(spath, AssetBundle.LoadFromFile(spath + ".asset"));
        ab = abSave[spath];
        return (T)ab.LoadAllAssets()[0];
#else
        return AssetDatabase.LoadAssetAtPath<T>(spath + ".prefab");
#endif
    }

    public static T loadAsset<T>(string spath,string pop) where T : Object
    {
#if AB_LOAD
        AssetBundle ab;
        if (!abSave.ContainsKey(spath))
            abSave.Add(spath, AssetBundle.LoadFromFile(spath + ".asset"));
        ab = abSave[spath];
        return ab.LoadAsset<T>(pop);
#else
        return AssetDatabase.LoadAssetAtPath<T>(spath + pop + ".prefab");
#endif
    }

    public static bool saveAsset<T>(T data, string path)
    {
        bool result = false;
        var obj = data as UnityEngine.Object;
        AssetDatabase.CreateAsset(obj, path);
        EditorUtility.SetDirty(obj);
        AssetDatabase.SaveAssets();
        if (File.Exists(path))
            result = true;
        return result;
    }
}
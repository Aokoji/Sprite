using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class AssetTool
{
    private static AssetTool instance;
    public static AssetTool Instance
    {
        get {
            if (instance == null)
                instance = new AssetTool();
            return instance;
        }
    }

    public static object loadAsset<T>(string spath)
    {
        var a = AssetDatabase.LoadAssetAtPath(spath, typeof(T));
        return a;
    }

    public static bool saveAsset<T>(T data,string path)
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


    public static NormalEventOneData loadGameData_NormalEventOneData(string spath)
    {
        return AssetDatabase.LoadAssetAtPath(spath, typeof(NormalEventOneData)) as NormalEventOneData;
    }

    public static ActorSaveData loadGameData(string spath)
    {
        return AssetDatabase.LoadAssetAtPath<ActorSaveData>(spath);
    }

    public static void saveGameData(ActorSaveData data,string path)
    {
        ActorSaveData obj =ScriptableObject.CreateInstance<ActorSaveData>();
        AssetDatabase.CreateAsset(obj, path);
        EditorUtility.SetDirty(obj);
        AssetDatabase.SaveAssets();
    }
}

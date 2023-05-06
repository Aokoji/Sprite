using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class AssetManager
{
    private static Dictionary<string, AssetBundle> abSave = new Dictionary<string, AssetBundle>();
    //ab包加载策略，由于体量小，暂用resource加载
    public static T loadAssetAB<T>(string itemname="", string bundlename="", string spath="") where T : Object
    {
#if UNITY_EDITOR
        return AssetDatabase.LoadAssetAtPath<T>(spath);
#else
        AssetBundle ab;
        if (!abSave.ContainsKey(bundlename))
            abSave.Add(bundlename, AssetBundle.LoadFromFile("StreamingAssets/" + bundlename));
        ab = abSave[bundlename];
        return (T)ab.LoadAsset(itemname);
#endif
    }

    public static T loadAsset<T>(string spath) where T : Object
    {
        return Resources.Load<T>(spath);
    }

    public static void saveJson(string jsname,object data)
    {
        var json = JsonUtility.ToJson(data);
        string path = Path.Combine(Application.persistentDataPath, jsname);
        Debug.Log(path);
        try
        {
            File.WriteAllText(path, json);
        }catch(System.Exception e)
        {
            Debug.LogError(e.ToString()+"*****  save json failed  :" + jsname);
        }
    }
    public static T loadJson<T>(string jsname)
    {
        string path = Path.Combine(Application.persistentDataPath, jsname);
        try
        {
            if (!File.Exists(path))
                return default(T);
            string obj = File.ReadAllText(path);
            T result = JsonUtility.FromJson<T>(obj);
            return result;
        }
        catch (System.Exception e)
        {
            Debug.LogError("read json failed  :" + jsname);
            throw e;
        }
    }

    public static bool saveAsset<T>(T data, string path)
    {
#if UNITY_EDITOR
        bool result = false;
        var obj = data as UnityEngine.Object;
        if(!File.Exists(path))
            AssetDatabase.CreateAsset(obj, path);
        //AssetDatabase.CreateAsset(obj, path);
        EditorUtility.SetDirty(obj);
        AssetDatabase.SaveAssets();
        if (File.Exists(path))
            result = true;
        return result;
#else
        return true;
#endif
    }
}

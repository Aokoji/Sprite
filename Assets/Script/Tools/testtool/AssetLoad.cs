using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoad : Singler<AssetLoad>
{
    public void loadPrefab(string sname,GameObject father)
    {

    }

    public GameObject loadPrefabPath(string path)
    {
        GameObject baseain = Resources.Load<GameObject>(path);
        var baseMain = Instantiate(baseain);
        return baseMain;
    }
    public GameObject loadPrefabPath(string path,GameObject father)
    {
        GameObject baseain = Resources.Load<GameObject>(path);
        var baseMain = Instantiate(baseain);
        baseMain.transform.SetParent(father.transform, false);
        return baseMain;
    }

    private IEnumerator LoadPrefab(E_UIPrefab pop)
    {
        yield return loadPrefabAsync();
    }

    private bool loadPrefabAsync()
    {
#if AB_LOAD
#else
        //return UnityEditor.AssetDatabase.LoadAssetAtPath<TargetJoint2D>(path);
#endif
        return true;
    }
}
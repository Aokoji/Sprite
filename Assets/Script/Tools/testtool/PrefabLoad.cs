using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoad : DDOL_Control<PrefabLoad>
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
}

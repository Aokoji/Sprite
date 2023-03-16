using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoad : MonoBehaviour
{
    public static GameObject loadPrefab(string path)
    {
        GameObject target = Resources.Load<GameObject>(path);
        var obj = ObjectUtil.Instantiate(target);
        return obj;
    }
}

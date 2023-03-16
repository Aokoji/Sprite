using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//对象工具库
public class ObjectUtil : MonoBehaviour
{
    private static Dictionary<RecycleObject, ObjectPool> pools = new Dictionary<RecycleObject, ObjectPool>();

    private static ObjectPool getObjectPool(RecycleObject recycle)
    {
        ObjectPool pool = null;
        if (pools.ContainsKey(recycle)) pool = pools[recycle];
        else
        {
            var obj = new GameObject(recycle.gameObject.name);
            pool = obj.AddComponent<ObjectPool>();
            pool.prefab = recycle;
            pools.Add(recycle, pool);
        }
        return pool;
    } 

    public static GameObject Instantiate(GameObject item,Vector2 pos)
    {
        GameObject obj;
        var recycle = item.GetComponent<RecycleObject>();
        if (recycle != null)
        {
            var pool = getObjectPool(recycle);
            obj = pool.nextObject(pos).gameObject;
        }
        else
        {
            obj = Instantiate(item);
            obj.transform.position = pos;
        }
        return obj;
    }
    public static GameObject Instantiate(GameObject item)
    {
        GameObject obj;
        var recycle = item.GetComponent<RecycleObject>();
        if (recycle != null)
        {
            var pool = getObjectPool(recycle);
            obj = pool.nextObject(Vector2.zero).gameObject;
        }
        else
        {
            obj = Object.Instantiate(item);
        }
        return obj;
    }
    public static void Destory(GameObject item)
    {
        var recycle = item.GetComponent<RecycleObject>();
        if (recycle != null) recycle.shutdown();
        else Destroy(item);
    }
}

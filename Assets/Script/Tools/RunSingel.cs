using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSingel : MonoBehaviour
{
    private static RunSingel instance;
    public static RunSingel Instance
    {
        get {
            if(null == instance)
            {
                var gameobj = Instantiate(new GameObject());
                gameobj.name = "Coroutine";
                instance = gameobj.AddComponent<RunSingel>();
                DontDestroyOnLoad(gameobj);
            }
            return instance;
        }
    }

    public void runTimer(IEnumerator obj)
    {
        StartCoroutine(obj);
    }

    /// <summary>
    /// 延时方法  参数 （延时float   回调action）
    /// </summary>
    public void laterDo(float time, Action action)
    {
        StartCoroutine(lateraction(time, action));
    }
    IEnumerator lateraction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }


    public void moveTo(GameObject obj, GameObject target, float time, int speed = 0)
    {
        runTimer(movetimer(obj, target, time, speed));
    }
    IEnumerator movetimer(GameObject obj, GameObject target, float time, int speed = 1)
    {
        var script = obj.GetComponent<Anim_Move>();
        if (script == null)
            script = obj.AddComponent<Anim_Move>();
        yield return null;
        script.setData(target, time, speed);
        script.starPlay();
    }
}

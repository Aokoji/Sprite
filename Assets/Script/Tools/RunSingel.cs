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
                var gameobj = new GameObject();
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

#region 移动方法
    /// <summary>
    ///  type 有一定加减速度运动，5中间值匀速，1作为贝塞尔参考区间倾斜开始慢--快
    /// </summary>
    public void moveTo(GameObject obj, GameObject target, float time, int type = 5)
    {
        runTimer(movetimer(obj, target, time, type));
    }
    IEnumerator movetimer(GameObject obj, GameObject target, float time, int type)
    {
        var script = obj.GetComponent<Anim_Move>();
        if (script == null)
            script = obj.AddComponent<Anim_Move>();
        script.setData(target, time, type);
        script.startPlay();
        yield return null;
    }
#endregion
}

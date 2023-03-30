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
    ///  
    /// </summary>
    public void moveTo(GameObject obj, Transform target, float time, Action callback = null)
    {
        runTimer(movetimer(obj, target, time, MoveType.moveto, Vector3.zero, callback));
    }
    public void moveToAll(GameObject obj, Transform target, MoveType typ, float time,Vector3 scale, Vector3 rotate,Action callback=null)
    {
        runTimer(movetimerAll(obj, target, time, typ, scale, rotate, callback));
    }
    public void moveToBezier(GameObject obj, Transform target, Vector3 bezier, float time, Action callback = null)
    {
        runTimer(movetimer(obj, target, time, MoveType.moveBezier, bezier, callback));
    }
    IEnumerator movetimer(GameObject obj, Transform target, float time, MoveType type, Vector3 bezier, Action callback = null)
    {
        var script = obj.GetComponent<Anim_Move>();
        if (script == null)
            script = obj.AddComponent<Anim_Move>();
        script.setData(target, time, type, bezier);
        script.startPlay(callback);
        yield return null;
    }
    IEnumerator movetimerAll(GameObject obj, Transform target, float time, MoveType type, Vector3 scale, Vector3 rotate,Action callback)
    {
        var script = obj.GetComponent<Anim_Move>();
        if (script == null)
            script = obj.AddComponent<Anim_Move>();
        script.setDataAll(target, time, type, scale, rotate);
        script.startPlay(callback);
        yield return null;
    }
    #endregion
}

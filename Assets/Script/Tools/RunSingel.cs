using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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
    public void moveTo(GameObject obj, Vector3 target, float time, Action callback=null)
    {
        runTimer(movetimer(obj, target, time, MoveType.moveto, Vector3.zero, callback));
    }
    public void moveToAll(GameObject obj, Vector3 target, MoveType typ, float time,Vector3 scale, Vector3 rotate,Action callback=null)
    {
        runTimer(movetimerAll(obj, target, time, typ, scale, rotate, callback));
    }
    public void moveToBezier(GameObject obj, Vector3 target, Vector3 bezier, float time, Action callback = null)
    {
        runTimer(movetimer(obj, target, time, MoveType.moveBezier, bezier, callback));
    }
    //强制
    public void moveToAll_force(GameObject obj, Vector3 target, MoveType typ, float time, Vector3 scale, Vector3 rotate, Action callback = null)
    {
        runTimer(movetimerAll(obj, target, time, typ, scale, rotate, callback, true));
    }

    IEnumerator movetimer(GameObject obj, Vector3 target, float time, MoveType type, Vector3 bezier, Action callback = null, bool force = false)
    {
        var script = obj.GetComponent<Anim_Move>();
        if (script == null)
            script = obj.AddComponent<Anim_Move>();
        script.setData(target, time, type, bezier);
        script.startPlay(callback);
        yield return null;
    }
    IEnumerator movetimerAll(GameObject obj, Vector3 target, float time, MoveType type, Vector3 scale, Vector3 rotate,Action callback,bool force=false)
    {
        var script = obj.GetComponent<Anim_Move>();
        if (script == null)
            script = obj.AddComponent<Anim_Move>();
        script.setDataAll(target, time, type, scale, rotate);
        script.startPlay(callback);
        yield return null;
    }
    #endregion
    #region 时间
    const float conectTime = 5;
    Action<DateTime> Callback;
    float curtime;
    public void getBeiJingTime(Action<DateTime> callback)
    {
        PanelManager.Instance.panelLock();
        PanelManager.Instance.LoadingShow(true, true);      //+++这个理论上不在这里，重连逻辑应该在调用处
        Callback = callback;
        curtime = 0;
        runTimer(gettime());
    }
    IEnumerator gettime()
    {
        string url = "https://www.baidu.com";
        UnityWebRequest web = new UnityWebRequest(url);
        DateTime starttime = DateTime.Now;
        yield return web.SendWebRequest();
        if (web.isDone && string.IsNullOrEmpty(web.error))
        {
            var res = web.GetResponseHeaders();
            string key = "DATE";
            string value = null;
            if (res != null && res.ContainsKey(key))
            {
                res.TryGetValue(key, out value);
            }
            PubTool.Log(value);       //GMT时间格式
            PanelManager.Instance.panelUnlock();
            PanelManager.Instance.LoadingShow(false);
            Callback(convertTime(value));
        }
        else
        {
            curtime++;
            if (curtime > conectTime)
            {
                PanelManager.Instance.showTips2("网络不佳，是否重新连接", () => { curtime = 0; runTimer(gettime()); }, Application.Quit);
            }
            else
            {
                Debug.LogError("网络错误，获取时间失败     重试");
                runTimer(gettime());
            }
        }
    }
    DateTime convertTime(string gmt)
    {
        DateTime dat = DateTime.MinValue;
        try
        {
            string pat = "";
            if (gmt.IndexOf("+0") != -1)
            {
                gmt = gmt.Replace("GMT", "");
                pat = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
            }
            if (gmt.ToUpper().IndexOf("GMT") != -1)
                pat = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
            if (pat != "")
            {
                dat = DateTime.ParseExact(gmt, pat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                dat = dat.ToLocalTime();
            }
            else
            {
                dat = Convert.ToDateTime(gmt);
            }
        }catch(Exception e)
        {
            Debug.Log(e);
        }
        return dat;
    }
    #endregion
}

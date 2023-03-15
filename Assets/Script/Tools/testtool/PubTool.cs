using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PubTool : DDOL_Control<PubTool>
{
    private bool ismotion = false;
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

    /// <summary>
    /// 慢动作0.5
    /// </summary>
    public void slowMotion()
    {
        if (!ismotion)
        {
            timeScaleSet(0.4f);
            laterDo(0.2f, delegate () { timeScaleReset(); });
        }
    }

    public void timeScaleSet(float v) { Time.timeScale = v; }
    public void timeScaleReset() { Time.timeScale = 1; }
}

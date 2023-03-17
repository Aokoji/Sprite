using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PubTool
{
    private static bool ismotion = false;
    /// <summary>
    /// 慢动作0.5
    /// </summary>
    public static void slowMotion()
    {
        if (!ismotion)
        {
            timeScaleSet(0.4f);
            RunSingel.Instance.laterDo(0.2f,timeScaleReset);
        }
    }
    public static void timeScaleSet(float v) { Time.timeScale = v; }
    public static void timeScaleReset() { Time.timeScale = 1; }
}

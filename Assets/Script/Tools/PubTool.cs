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

    public static Color getColor(Vector3 vec)
    {
        return new Color(vec.x / 255, vec.y / 255, vec.z / 255);
    }
    public static string timeTranslate(int minutes)
    {
        string str = "";
        int h = minutes / 60;
        if (h > 0)
        {
            str = h + " 小时 ";
        }
        str=str+ minutes % 60 + " 分钟";
        return str;
    }
    public static string timeTranslateSeconds(int second)
    {
        if (second < 60)
            return second+"秒";
        return timeTranslate(second / 60);
    }
    public static void Log(string log)
    {
        Debug.Log(log);
    }
    public static void LogError(string log)
    {
        Debug.LogError(log);
    }
}
public enum MoveType
{
    moveto,
    moveAll,    //scale rotate
    moveAll_STF,    //slow to fase
    moveAll_FTS,    //
    moveBezier, //all
}

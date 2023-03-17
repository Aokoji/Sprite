using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LogTool : MonoBehaviour
{
    public static void Log(string context)
    {
        if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");       //根目录存在Logs文件夹   不存在则创建
        string path = Path.Combine("Logs", "sysLog.txt");
        string mess = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "\n    " + context + "\n";
        File.AppendAllText(path, mess);
    }
    public static void aLog(string context)
    {
        if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");       // 不存在则创建
        string path = Path.Combine("Logs", "actionLog.txt");
        string mess = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "\n    " + context + "\n";
        File.AppendAllText(path, mess);
    }
    public static void LogWarn(string context)
    {
        if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");       //根目录存在Logs文件夹   不存在则创建
        string path = Path.Combine("Logs", "sysLog.txt");
        string mess = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + "\n    *w*"  + context + "\n";
        File.AppendAllText(path, mess);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CSVLoad
{

    //private static string buffpath = "Assets/Resources/csv//buffMessage.csv";//buff信息
    

    //      --------------------------------------      其他方法         ---------------------------------------------
    public static List<string[]> loadCSV(string path)
    {
        List<string[]> data = new List<string[]>();
        StreamReader sr = null;
        if (File.Exists(path))
        {
            sr = File.OpenText(path);
        }
        else
        {
            Debug.LogError("读取静态数据异常!!!!" + path);
            return null;
        }
        string str;
        while ((str = sr.ReadLine()) != null)
        {
            data.Add(str.Split(','));
        }
        sr.Close();
        sr.Dispose();
        if (data == null || data.Count <= 0)
        {
            Debug.LogError("读取错误，空数据：" + path);
        }
        return data;
    }

}

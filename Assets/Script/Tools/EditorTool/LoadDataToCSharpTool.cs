using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Timers;

public class LoadDataToCSharpTool :Editor
{

    //static List<TableData> dataTable=new List<TableData>();
    static string defaultPath = "Data/Table";
    static string csharpPath = "/Script/Game/Data/testdata/";
    static string assetPath = "Assets/config/tabledata";
    static List<TableData> tables;

    //暂且只用来创建CSharp 因为数据需要编译好的cs文件
    [MenuItem("dataTool/createDefaultPathDataCSharp")]
    public static void createDefaultPathDataCSharp()
    {
        tables = new List<TableData>();
        //读文件
        readAndCreateFile();
        //创建.cs文件
        rewriteFileAndSaveData();
    }
    //导入数据
    [MenuItem("dataTool/importDefaultPathData")]
    public static void importDefaultPathData()
    {
        
    }

    static void readAndCreateFile()
    {
        DirectoryInfo direct = new DirectoryInfo(defaultPath);
        FileInfo[] files = direct.GetFiles("*", SearchOption.AllDirectories);
        string localpath;
        StreamReader sr;
        StreamWriter sw;
        TableData data;
        StringBuilder scriptStr;
        Debug.Log("readingfiles: " + files.Length);
        //读文件
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".csv"))
            {
                scriptStr = new StringBuilder();
                localpath = files[i].FullName;
                data = new TableData();
                //检查csv文件
                if (File.Exists(localpath))
                {
                    sr = File.OpenText(localpath);
                }
                else
                {
                    Debug.LogError("读取静态数据异常!!!!" + localpath);
                    return;
                }
                data.fieldName = files[i].Name.Replace(".csv", ".cs");
                data.scriptName = files[i].Name.Replace(".csv", "");
                //创建cs文件
                if (!File.Exists(Application.dataPath + csharpPath + data.fieldName))
                {
                    Debug.LogWarning("New CS File Name : " + files[i].Name);
                    sw = File.CreateText(Application.dataPath + csharpPath + data.fieldName);
                    sw.Close();
                    sw.Dispose();
                }
                string str;
                int index = 0;
                string[] csNames = null;
                string[] csTypes;
                scriptStr.Append(CS_str1);
                scriptStr.Append(string.Format(CS_strClass1, data.scriptName));
                scriptStr.Append(CS_str2);
                //读数据
                while ((str = sr.ReadLine()) != null)
                {
                    if (index == 0)
                    {
                        csNames = str.Split(',');
                        index++;
                        continue;
                    }
                    if (index == 1)
                    {
                        csTypes = str.Split(',');
                        for (int k = 0; k < csTypes.Length; k++)
                        {
                            scriptStr.Append(string.Format(CS_strMember1, csTypes[k], csNames[k]));
                        }
                        break;
                    }
                }
                scriptStr.Append(CS_str3);
                scriptStr.Append(string.Format(CS_strSelf1, data.scriptName));
                scriptStr.Append(CS_str4);
                data.context = scriptStr.ToString();
                tables.Add(data);
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
    }
    static void rewriteFileAndSaveData()
    {
        for(int i = 0; i < tables.Count; i++)
        {
            File.WriteAllText(Application.dataPath + csharpPath + tables[i].fieldName, tables[i].context);
            Debug.Log("write success" + tables[i].fieldName);
        }
        Debug.Log("Success !  All files is : " + tables.Count);
    }
    static void readToTypeData()
    {
        DirectoryInfo direct = new DirectoryInfo(defaultPath);
        FileInfo[] files = direct.GetFiles("*", SearchOption.AllDirectories);
        string localpath;
        StreamReader sr;
        string filename;
        Debug.Log("readingfiles: " + files.Length);
        //读文件
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".csv"))
            {
                localpath = files[i].FullName;
                //检查csv文件
                if (File.Exists(localpath))
                    sr = File.OpenText(localpath);
                else
                {
                    Debug.LogError("读取静态数据异常!!!!" + localpath);
                    return;
                }
                filename = files[i].Name.Replace(".csv", "");
                string str;
                string[] types;
                int index = 0;
                var obj = Activator.CreateInstance(Type.GetType(filename));
                ScriptableObject asset = CreateInstance(filename);
                //读数据
                while ((str = sr.ReadLine()) != null)
                {
                    //跳过前两行
                    if (index < 2)
                    {
                        if(index==1)
                            types= str.Split(',');
                        index++;
                        continue;
                    }
                    string[] context = str.Split(',');
                    for(int k = 0; k < context.Length; k++)
                    {
                        
                    }
                }

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
    }
    struct TableData
    {
        public string fieldName;
        public string scriptName;
        public string context;
    }
    /// <summary>
    /// 实体类模板
    /// </summary>
    static string CS_str1 = "using System.Collections.Generic;\r";
    static string CS_str2 = ":ScriptableObject\r{\r\tpublic class t_data\r\t{\r";
    static string CS_str3 = "\t}\r\tpublic Dictionary<int, t_data> _data;\r\tpublic bool isloaded;\r";
    static string CS_str4 = "\r\t{\r\t\tisloaded = false;\r\t\tpraseData();\r\t}\r}";       //这里有一个结束括号  -- } ---
    static string CS_strSelf1 = "\tpublic {0}()";
    static string CS_strClass1 ="public class {0}";
    static string CS_strMember1 = "\t\tpublic {0} {1};\r";
}

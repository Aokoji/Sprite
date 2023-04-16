using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class LoadDataToCSharpTool :Editor
{
    static string defaultPath = "Data/Table";
    static string csharpPath = "/Script/Game/Data/datacs/";
    static string csharpPath0 = "Assets/Script/Game/Data/datacs"; //删文件用的
    static string assetPath0 = "Assets/config/tabledata";//删文件用的
    static string assetPath = "Assets/config/tabledata/";
    static string tableconfigPath = "/Script/Game/Data/TableConfig.cs";
    static List<TableData> tables;
    static Dictionary<string, byte> willDeleteNames;

    //暂且只用来创建CSharp 因为数据需要编译好的cs文件
    [MenuItem("dataTool/createDefaultPathDataCSharp")]
    public static void createDefaultPathDataCSharp()
    {
        tables = new List<TableData>();
        willDeleteNames = new Dictionary<string, byte>();
        //读文件
        readAndCreateFile();
        //创建.cs文件
        rewriteFileAndSaveData();
        tables = null;
        willDeleteNames = null;
    }
    //导入数据
    [MenuItem("dataTool/importDefaultPathData")]
    public static void importDefaultPathData()
    {
        willDeleteNames = new Dictionary<string, byte>();
        readToTypeData();
        willDeleteNames = null;
    }

    static void readAndCreateFile()
    {
        //读所有csv
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
                data.fieldName = "Config_" + files[i].Name.Replace(".csv", ".cs");
                data.scriptName = files[i].Name.Replace(".csv", "");
                //创建cs文件
                if (!File.Exists(Application.dataPath + csharpPath + data.fieldName))
                {
                    Debug.LogWarning("New CS File Name : " + files[i].Name);
                    sw = File.CreateText(Application.dataPath + csharpPath + data.fieldName);
                    sw.Close();
                    sw.Dispose();
                }
                //记录已有的文件   还有meta
                willDeleteNames.Add(data.fieldName, 0);
                willDeleteNames.Add(data.fieldName+".meta", 0);
                string str;
                int index = 0;
                string[] csNames = null;
                string[] csTypes;
                string context = CS_strclass.Replace("@CLASS", "Config_" + data.scriptName);
                context = context.Replace("@FILE", data.scriptName);
                scriptStr.Append(context);

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
                            string s = CS_strMember1.Replace("@CLASS", csTypes[k]);
                            scriptStr.Append(s.Replace("@MEMBER", csNames[k]));
                        }
                        break;
                    }
                }
                scriptStr.Append("}");
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
        //重写config
        StringBuilder str = new StringBuilder();
        str.Append(CS_configStr1);

        for (int i = 0; i < tables.Count; i++)
        {
            File.WriteAllText(Application.dataPath + csharpPath + tables[i].fieldName, tables[i].context);
            str.Append(CS_configMember.Replace("@FILE",tables[i].scriptName));
            Debug.Log("write success" + tables[i].fieldName);
        }
        str.Append(CS_configStr2);
        for (int i = 0; i < tables.Count; i++)
            str.Append(CS_configMember_des.Replace("@FILE", tables[i].scriptName));
        str.Append(CS_configStr3);
        File.WriteAllText(Application.dataPath + tableconfigPath, str.ToString());
        Debug.Log("write config success");
        //清理 不用的config文件
        DirectoryInfo direct = new DirectoryInfo(csharpPath0);
        FileInfo[] files = direct.GetFiles("*", SearchOption.AllDirectories);
        for (int i=0; i < files.Length; i++)
        {
            if (!willDeleteNames.ContainsKey(files[i].Name))
                File.Delete(Application.dataPath + csharpPath + files[i].Name);
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
        string filepath;
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
                filepath = assetPath + filename + "_data.asset";
                // 记录不删除
                willDeleteNames.Add(filename + "_data.asset", 0);
                willDeleteNames.Add(filename + "_data.asset.meta", 0);
                string str;
                string[] names=null;
                string[] types = null;
                int index = 0;
                //创建asset
                ScriptableObject asset = CreateInstance("Config_"+filename);
                //读数据
                while ((str = sr.ReadLine()) != null)
                {
                    //跳过前两行
                    if (index < 2)
                    {
                        if (index == 0)
                            names = str.Split(',');
                        if (index == 1)
                            types = str.Split(',');
                        index++;
                        continue;
                    }
                    string[] context = str.Split(',');

                    var obj = Activator.CreateInstance(Type.GetType(filename));
                    Type target = obj.GetType();
                    for (int k = 0; k < context.Length; k++)
                    {
                        //数据赋值
                        target.GetField(names[k]).SetValue(obj, asset.GetType().GetMethod("parse").Invoke(asset, new object[] { context[k], types[k] }));
                    }
                    var func = asset.GetType().GetMethod("addContent");
                    func.Invoke(asset, new object[]{ obj });
                }
                if(File.Exists(filepath))
                    AssetDatabase.DeleteAsset(filepath);
                AssetDatabase.CreateAsset(asset, filepath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
        //删无用文件asset
        direct = new DirectoryInfo(assetPath0);
        files = direct.GetFiles("*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (!willDeleteNames.ContainsKey(files[i].Name))
                File.Delete(assetPath + files[i].Name);
        }
        Debug.Log("import Success !");
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
    static string CS_strclass = "using System.Collections.Generic;\rusing UnityEditor;\rusing System;\rpublic class @CLASS : AssetData\r{\r\tpublic List<@FILE> asset = new List<@FILE>();\r\tpublic void addContent(@FILE item)\r\t{\r\t\tasset.Add(item);\r\t}\r" +
        "\tpublic static bool isloaded;\r\tpublic static Dictionary<int, @FILE> _data { get; private set; }\r\tpublic void init()\r\t{\r\t\t_data = new Dictionary<int, @FILE>();\r\t\tfor (int i = 0; i < asset.Count; i++){\r\t\t\t_data.Add(asset[i].id, asset[i]);\r\t\t}\r\t\tasset = null;\r\t\tisloaded = true;\r\t}\r\tpublic static @FILE getOne(int id)\r\t{\r\t\tif (_data.ContainsKey(id))\r\t\t\treturn _data[id];\r\t\telse\r\t\t\tthrow new Exception(\"not find\" + id);\r\t}\r\tpublic static void Dispose()\r\t{\r\t\t_data = null;\r\t}\r}\r" +
        "[Serializable]\rpublic class @FILE\r{\r";
    static string CS_strMember1 = "\tpublic @CLASS @MEMBER;\r";
			
    //table config模板
    static string CS_configStr1 = "using System.Collections;\rusing UnityEditor;\rpublic class TableConfig\r{\r\tstring ASSETPATH = \"Assets/config/tabledata/\";\r\tpublic bool loadsuccess;\r\tpublic void init()\r\t{\r\t\tloadsuccess = false;\r\t\tRunSingel.Instance.runTimer(loadData());\r\t}\r\tIEnumerator loadData()\r\t{";
    static string CS_configStr2 = "\r\t\tloadsuccess = true;\r\t}\r\tpublic void Dispose()\r\t{";
    static string CS_configStr3 = "\r\t}\r}";

    static string CS_configMember = "\r\t\tAssetDatabase.LoadAssetAtPath<Config_@FILE>(ASSETPATH + \"@FILE_data.asset\").init();\r\t\twhile (!Config_@FILE.isloaded)\r\t\t\tyield return null;";
    static string CS_configMember_des = "\r\t\tConfig_@FILE.Dispose();";
}

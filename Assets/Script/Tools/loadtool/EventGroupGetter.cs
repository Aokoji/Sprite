using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class EventGroupGetter : CSingel<EventGroupGetter>
{
    private static string normalOnePath = "/Resources/asset/oneevent/";
    //private static string propertyPath = "/Resources/asset/chance/";

    private static Dictionary<int, NormalEventData> normalEventList = new Dictionary<int, NormalEventData>();       //普通事件列表    csv配表
    private static Dictionary<int, NormalEventOneData> normalEventOneList = new Dictionary<int, NormalEventOneData>();      //单窗口事件     asset读取
    private static Dictionary<int, PropertyChangeData> propertyChangeList = new Dictionary<int, PropertyChangeData>();      //操作改变    asset读取   csv
    private static Dictionary<int, BuffData> bufflist = new Dictionary<int, BuffData>();        //csv配表
    private static Dictionary<int, TaskPropertyData> tasklist = new Dictionary<int, TaskPropertyData>();        //csv配表
    private static Dictionary<int, ItemData> itemlist = new Dictionary<int, ItemData>();        //csv配表
    private static Dictionary<int, SkillPropertyData> skillProlist = new Dictionary<int, SkillPropertyData>();        //csv配表

    public static bool isloaded;
    public void loadEventAsset()
    {
        isloaded = false;
        //读表  加载
        RunSingel.Instance.runTimer(loadAssetsToDictionary());
    }

    //只有csv掉这个
    public static void addBuff(int id,BuffData data){ bufflist.Add(id, data); }
    public static void addNormalEvent(int id, NormalEventData data){ normalEventList.Add(id, data); }
    public static void addTask(int id, TaskPropertyData data){ tasklist.Add(id, data); }
    public static void addProChange(int id, PropertyChangeData data){ propertyChangeList.Add(id, data); }
    public static void addSkillProperty(int id, SkillPropertyData data){ skillProlist.Add(id, data); }

    private static void loadNormalEventOne()
    {
        var files = Directory.GetFiles(Application.dataPath + normalOnePath, "*.asset");
        foreach(var path in files)
        {
            NormalEventOneData data = AssetTool.loadGameData_NormalEventOneData(path.Replace(Application.dataPath, "Assets"));
            normalEventOneList.Add(data.id, data);
        }
    }

    IEnumerator loadAssetsToDictionary()
    {
        CSVLoad.initLoadAllData();
        loadNormalEventOne();
        isloaded = true;
        //loadpropertyChangeList();
        yield return null;
    }



    public static NormalEventData getNormalEventGroup(int id)
    {
        //根据其中的 单事件id 读取事件字典的事件填充
        if (!normalEventList.ContainsKey(id))
        {
            Debug.LogError("获取事件错误！不存在的id" + id);
            return null;
        }
        return normalEventList[id];
    }
    public static NormalEventOneData getNormalEventOne(int id)
    {
        //根据其中的 单事件id 读取事件字典的事件填充
        if (!normalEventOneList.ContainsKey(id))
        {
            Debug.LogError("获取事件错误！不存在的id" + id);
            return null;
        }
        return normalEventOneList[id];
    }
    public static PropertyChangeData getPropertyData(int id)
    {
        //根据其中的 单事件id 读取事件字典的事件填充
        if (!propertyChangeList.ContainsKey(id))
        {
            Debug.LogError("获取改变错误！不存在的id" + id);
            return null;
        }
        return propertyChangeList[id];
    }
    //任务
    public static TaskPropertyData getTaskData(int id)
    {
        if (!tasklist.ContainsKey(id))
        {
            Debug.LogError("获取改变错误！不存在的id" + id);
            return null;
        }
        return tasklist[id];
    }
    //物品
    public static ItemData getItemData(int id)
    {
        if (!itemlist.ContainsKey(id))
        {
            Debug.LogError("获取改变错误！不存在的id" + id);
            return null;
        }
        return itemlist[id];
    }
    //buff
    public static BuffData getBuffData(int id)
    {
        if (!bufflist.ContainsKey(id))
        {
            Debug.LogError("获取改变错误！不存在的id" + id);
            return null;
        }
        return bufflist[id];
    }
    //buff
    public static SkillPropertyData getSkillProData(int id)
    {
        if (!skillProlist.ContainsKey(id))
        {
            Debug.LogError("获取skillPor错误！不存在的id" + id);
            return null;
        }
        return skillProlist[id];
    }
}

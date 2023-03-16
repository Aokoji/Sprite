using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CSVLoad
{

    private static string buffpath = "Assets/Resources/csv//buffMessage.csv";//buff信息
    private static string normalGroupPath = "Assets/Resources/csv//normalGroup.csv";//buff信息
    private static string taskPath = "Assets/Resources/csv//taskMsg.csv";//buff信息
    private static string proChangePath = "Assets/Resources/csv//event_Chance.csv";// 属性改变信息

    private static string relationPath1 = "Assets/Resources/csv//relation_1.csv";
    private static string relationPath2 = "Assets/Resources/csv//relation_2.csv";
    private static string relationPath3 = "Assets/Resources/csv//relation_3.csv";

    private static string skillProPath = "Assets/Resources/csv//skillProperty.csv";

    public static void initLoadAllData()
    {
        loadbuff();
        loadNormalEventGroup();
        loadTask();
        loadPropertyChange();
        loadRelationDefault();
        loadSkillPropertyData();
    }

    //      --------------------------------------      需要加载的部分         ---------------------------------------------
    private static void loadbuff()
    {
        var allbuff = loadCSV(buffpath);
        BuffData data;
        int count = 0;
        int curindex;
        foreach (var item in allbuff)
        {
            if (count == 0)
            {
                count++;
                continue;
            }
            data = new BuffData();
            curindex = 0;
            data.id= int.Parse(item[curindex++]);
            data.sname = item[curindex++];
            data.describe = item[curindex++];
            if (item[curindex] != "0")
            {
                string[] str = item[curindex].Split('%');
                foreach (var i in str)
                    data.effectPro.Add(int.Parse(i));
            }
            curindex++;
            if (item[curindex] != "0")
            {
                string[] str = item[curindex].Split('%');
                foreach (var i in str)
                    data.effectNum.Add(int.Parse(i));
            }
            curindex++;
            if (item[curindex] != "0")
            {
                string[] str = item[curindex].Split('%');
                foreach (var i in str)
                    data.effectPercente.Add(int.Parse(i));
            }
            curindex++;
            if (item[curindex] != "0")      //在add的时候才会去计算float
            {
                string[] str = item[curindex].Split('%');
                foreach (var i in str)
                    data.floatNum.Add(int.Parse(i));
            }
            curindex++;
            data.stype = item[curindex++];
            data.duration = int.Parse(item[curindex++]);
            data.isSpecial = item[curindex++] == "0";
            if (data.isSpecial)
            {
                data.spID = int.Parse(item[curindex++]);
                data.spType = int.Parse(item[curindex++]);
                data.spSquare = int.Parse(item[curindex++]);
            }
            else
                curindex += 3;
            //池一定要在最后   没什么特殊，只是想它在最后
            if (!string.IsNullOrEmpty(item[curindex]) && item[curindex] != "0")      //在add的时候才会去计算float
            {
                string[] str = item[curindex].Split('%');
                foreach (var i in str)
                    BuffPoolGetter.addToBuffPool(int.Parse(i), data.id);
            }
            EventGroupGetter.addBuff(data.id, data);
        }
    }
    //加载  单窗口做选择的事件
    private static void loadNormalEventGroup()
    {
        var data = loadCSV(normalGroupPath);
        NormalEventData group;
        int count = 0;
        int curindex;
        int length;
        foreach (var item in data)
        {
            if (count == 0)
            {
                count++;
                continue;
            }
            length = item.Length;
            curindex = 0;
            group = new NormalEventData();
            group.actionID = int.Parse(item[curindex]);
            curindex += 2;
            for (; curindex < length; curindex++)
            {
                if (item[curindex].Equals("**")) break; 
                group.actionList.Add(int.Parse(item[curindex]));
            }
            curindex++;
            //后续的铺垫
            EventGroupGetter.addNormalEvent(group.actionID, group);
            count++;
        }
    }

    //任务
    private static void loadTask()
    {
        var data = loadCSV(taskPath);
        int count = 0;
        int curindex;
        TaskPropertyData task;
        foreach (var item in data)
        {
            if (count == 0)
            {
                count++;
                continue;
            }
            curindex = 0;
            task = new TaskPropertyData();
            task.id= int.Parse(item[curindex++]);
            task.iscompleted = item[curindex++] == "1";
            task.isauto = item[curindex++] == "1";
            task.chapter = item[curindex++];
            task.describe = item[curindex++];
            task.complete_describe = item[curindex++];
            task.taskType = int.Parse(item[curindex++]);
            task.difficulty = int.Parse(item[curindex++]);
            task.remainTime = int.Parse(item[curindex++]);
            task.isCheckRefresh = item[curindex++] == "1";
            task.nextTaskID = int.Parse(item[curindex++]);
            task.completeNum = int.Parse(item[curindex++]);
            task.arriveAny = int.Parse(item[curindex++]);
            task.attackAny = int.Parse(item[curindex++]);
            task.whatId = int.Parse(item[curindex++]);
            task.becomeNum = int.Parse(item[curindex++]);
            task.choose_describe = item[curindex++];
            task.choose_toID = item[curindex++];
            task.rewardID = item[curindex++];
            task.rewardNum = item[curindex++];
            EventGroupGetter.addTask(task.id, task);
            count++;
        }
    }
    private static void loadPropertyChange()
    {
        var data = loadCSV(proChangePath);
        int count = 0;
        int curindex;
        PropertyChangeData pro;
        foreach (var item in data)
        {
            if (count == 0)
            {
                count++;
                continue;
            }
            curindex = 0;
            pro = new PropertyChangeData();
            pro.id = int.Parse(item[curindex++]);
            pro.isPro = item[curindex++] == "1";
            pro.proID = item[curindex++];
            pro.addNum = item[curindex++];
            pro.isBuff = item[curindex++]=="1";
            pro.buffID = item[curindex++];
            pro.randomNum = int.Parse(item[curindex++]);
            pro.randomPool = int.Parse(item[curindex++]);
            pro.isHideBuff = item[curindex++]=="1";
            pro.hideBuffID = item[curindex++];
            pro.labelID = int.Parse(item[curindex++]);
            EventGroupGetter.addProChange(pro.id, pro);
            count++;
        }
    }

    //relation
    private static void loadRelationDefault()
    {
        var data = loadCSV(relationPath1);

        data = loadCSV(relationPath2);
        data = loadCSV(relationPath3);

    }
    //人物生存与天赋技能
    private static void loadSkillPropertyData()
    {
        var allskillPro = loadCSV(skillProPath);
        SkillPropertyData data;
        int count = 0;
        int curindex;
        foreach (var item in allskillPro)
        {
            if (count == 0)
            {
                count++;
                continue;
            }
            curindex = 0;
            data = new SkillPropertyData();
            data.id = int.Parse(item[curindex++]);
            data.sname = item[curindex++];
            data.expBase = int.Parse(item[curindex++]);
            data.expGap = int.Parse(item[curindex++]);
            data.initData();
            EventGroupGetter.addSkillProperty(data.id, data);
            count++;
        }
    }

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

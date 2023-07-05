using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class DirectionHelp : UIBase
{
    public List<Button> allbutton = new List<Button>();

    Action<int> callback;
    t_ExplorMapHelp _data;

    public void initData(t_ExplorMapHelp map,Action<int> action)
    {
        callback = action;
        _data = map;
        for(int i = 0; i < allbutton.Count; i++)
        {
            int index = i;
            allbutton[i].onClick.AddListener(() => { callback?.Invoke(index); });
        }
        //初始化字典
        string[] str = _data.connection.Split('|');
        for(int i = 0; i < str.Length; i++)
        {
            string[] pos = str[i].Split('-');
            int key = int.Parse(pos[0]);
            if (!posmessage.ContainsKey(key))
                posmessage[key] = new List<int>();
            for(int k = 1; k < pos.Length; k++)
                posmessage[key].Add(int.Parse(pos[k]));
        }
    }
    Dictionary<int, List<int>> posmessage = new Dictionary<int, List<int>>();
    public void showCurPosDirect(int curpos)
    {
        foreach (var i in allbutton)
            i.gameObject.SetActive(false);
        if (posmessage.ContainsKey(curpos))
        {
            foreach(var i in posmessage[curpos])
            {
                if (i > curpos)
                {
                    if (i == curpos + 1)
                    {
                        allbutton[(int)ExplorMovingPanel.direct.right].gameObject.SetActive(true);
                        continue;
                    }
                    if (i == curpos + _data.mapwidth)
                    {
                        allbutton[(int)ExplorMovingPanel.direct.up].gameObject.SetActive(true);
                        continue;
                    }
                    if (i > curpos + _data.mapwidth)
                    {
                        allbutton[(int)ExplorMovingPanel.direct.ru].gameObject.SetActive(true);
                        continue;
                    }
                    else
                    {
                        allbutton[(int)ExplorMovingPanel.direct.lu].gameObject.SetActive(true);
                        continue;
                    }
                }
                else
                {
                    if (i == curpos - 1)
                    {
                        allbutton[(int)ExplorMovingPanel.direct.left].gameObject.SetActive(true);
                        continue;
                    }
                    if (i == curpos - _data.mapwidth)
                    {
                        allbutton[(int)ExplorMovingPanel.direct.down].gameObject.SetActive(true);
                        continue;
                    }
                    if (i > curpos - _data.mapwidth)
                    {
                        allbutton[(int)ExplorMovingPanel.direct.rd].gameObject.SetActive(true);
                        continue;
                    }
                    else
                    {
                        allbutton[(int)ExplorMovingPanel.direct.ld].gameObject.SetActive(true);
                        continue;
                    }
                }
            }
        }
        gameObject.SetActive(true);
    }
}

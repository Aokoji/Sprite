using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITool_ScrollView :MonoBehaviour {

	public GridLayoutGroup content;

    private List<GameObject> childs = new List<GameObject>();
    private string loadPath;
    int rowcount;
    int itemwidth;
    int itemheight;

    public void initConfig(int width,int heigh)
    {
        itemwidth = width;
        itemheight = heigh;
        content.cellSize = new Vector2(itemwidth, itemheight);
        rowcount = (int)(GetComponent<RectTransform>().sizeDelta.x / (width + content.spacing.x));
    }

	public void addNewItem(GameObject obj)
    {
        obj.transform.SetParent(content.transform, false);
        childs.Add(obj);
        obj.transform.SetAsLastSibling();
        //return obj;
    }
    public void reCalculateHeigh()
    {
        int high = childs.Count / rowcount;
        if ((float)childs.Count % rowcount > 0)
        {
            high++;
        }
        content.GetComponent<RectTransform>().sizeDelta =new Vector2(0, high* (itemheight + content.spacing.x));
    }

    public void removeLastItem()
    {
        if (childs.Count == 0)
        {
            Debug.LogError("列表已空");
            return;
        }
        GameObject obj = null;
        for(int i=childs.Count-1;i>0;i--)
        {
            if (childs[i].activeSelf)
            {
                obj = childs[i];
                break;
            }
        }
        if (null == obj)
        {
            Debug.LogError("列表已空");
            return;
        }
        obj.SetActive(false);
    }
    public void recycleAll()
    {
        foreach (var i in childs)
        {
            i.SetActive(false);
        }
    }
    public void removeAll()
    {
        foreach(var i in childs)
        {
            GameObject.Destroy(i);
        }
        childs = new List<GameObject>();
    }
    public void removeOne(GameObject obj,bool real=false)
    {
        obj.SetActive(false);
        if (real)
        {
            foreach(var i in childs)
                if (i == obj)
                {
                    GameObject.Destroy(i);
                    break;
                }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITool_ScrollView :MonoBehaviour {

	public GameObject content;

    private List<GameObject> childs;
    private string loadPath;
    public void init(int stype)
    {
        childs = new List<GameObject>();
    }

	public GameObject addNewItem()
    {
        GameObject obj=null;
        foreach(var i in childs)
        {
            if (!i.activeSelf)
            {
                obj = i;
                break;
            }
        }
        if (null == obj)
        {
            //load path
            obj = new GameObject();
            obj.transform.SetParent(content.transform, false);
            childs.Add(obj);
        }
        obj.transform.SetAsLastSibling();
        return obj;
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

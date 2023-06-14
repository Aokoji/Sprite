using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITool_ScrollView :MonoBehaviour {

	public GridLayoutGroup content;

    private List<GameObject> childs = new List<GameObject>();
    Queue<GameObject> childsQue = new Queue<GameObject>();
    GameObject prefab;  //原型
    private string loadPath;
    int rowcount;
    int itemwidth;
    int itemheight;

    //手动调用 自动计算行数量
    public void initConfig(int width,int heigh,GameObject instance)
    {
        itemwidth = width;
        itemheight = heigh;
        prefab = instance;
        content.cellSize = new Vector2(itemwidth, itemheight);
        rowcount = (int)(GetComponent<RectTransform>().sizeDelta.x / (width + content.spacing.x));
    }
    //外部调用
    public GameObject addItemDefault()
    {
        GameObject obj;
        if (childsQue.Count > 0)
            obj = childsQue.Dequeue();
        else
        {
            obj = Instantiate(prefab);
            obj.transform.SetParent(content.transform, false);
        }
        obj.SetActive(true);
        obj.transform.SetAsLastSibling();
        childs.Add(obj);
        return obj;
    }
    //重新计算高度（变动后要调用
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
        var obj = childs[childs.Count - 1];
        childs.RemoveAt(childs.Count - 1);
        obj.SetActive(false);
        childsQue.Enqueue(obj);
    }
    public void recycleAll()
    {
        foreach (var i in childs)
        {
            i.SetActive(false);
            childsQue.Enqueue(i);
        }
        childs.Clear();
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

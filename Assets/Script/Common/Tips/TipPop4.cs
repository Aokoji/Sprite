using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPop4 : TipsBase
{
    public GameObject cloneBar;
    public Transform parent;
    Queue<GameObject> objpool = new Queue<GameObject>();
    List<GameObject> showing = new List<GameObject>();
    float xpos = 40; //半个
    public override void init(object whatthing)
    {
        allowClick = false;
        var items = whatthing as List<ItemData>;
        initEvent();
        cloneBar.SetActive(false);
        Vector3 startpos = Vector3.zero;
        startpos.x -= (items.Count - 1) * xpos;
        foreach(var i in showing)
        {
            i.SetActive(false);
            objpool.Enqueue(i);
        }
        showing.Clear();
        foreach (var i in items)
        {
            var obj = getoneObj();
            obj.transform.SetParent(parent);
            obj.transform.localPosition = startpos;
            obj.transform.localScale = Vector3.one;
            obj.GetComponentInChildren<Text>().text = "x" + i.num;
            obj.GetComponentInChildren<Image>().sprite = GetSprite(A_AtlasNames.itemsIcon.ToString(), Config_t_items.getOne(i.id).iconName);
            obj.SetActive(true);
            showing.Add(obj);
            startpos.x += 2 * xpos;
        }
    }
    GameObject getoneObj()
    {
        if (objpool.Count > 0)
            return objpool.Dequeue();
        else
            return Instantiate(cloneBar);
    }
    public override void play()
    {
        AnimationTool.playAnimation(gameObject, "showtip1", false, () => { gameObject.SetActive(false); });
    }
}

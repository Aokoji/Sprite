using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemExplain : UIBase
{
    public GameObject messBar;
    public Text title;
    public Text explain;
    public Button closeBtn;

    public void initShow(int itemid,Vector3 pos)
    {
        var config = Config_t_items.getOne(itemid);
        title.text = config.sname;
        explain.text = config.describe;
        messBar.transform.position = pos;
        closeBtn.onClick.AddListener(closeclick);
        AnimationTool.playAnimation(gameObject, "showExplainTip");
    }
    void closeclick()
    {
        Destroy(gameObject);
    }
}

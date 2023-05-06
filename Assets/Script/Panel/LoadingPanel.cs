using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel:UIBase
{
    public Text showtext;
    public void Loading(bool show,bool isconect)
    {
        gameObject.SetActive(show);
        if(show)
            showtext.text = isconect ? "连接中..." : "加载中...";
    }
}

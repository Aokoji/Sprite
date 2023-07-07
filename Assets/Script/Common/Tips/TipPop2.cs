using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPop2 : TipsBase
{
    public Button confirm;
    public Button cancel;
    public Text str2;

    private void Start()
    {
        confirm.onClick.AddListener(confirmEvent);
        cancel.onClick.AddListener(cancelEvent);
    }
    public override void setString2(string str)
    {
        base.setString2(str);
        str2.text = str;
    }
    public override void play()
    {
        AnimationTool.playAnimation(gameObject, "showtip2",false,()=> { allowClick = true; });
    }
    void confirmEvent()
    {
        if (!allowClick) return;
        allowClick = false;
        callback?.Invoke();
        close();
    }
    void cancelEvent()
    {
        if (!allowClick) return;
        allowClick = false;
        callback2?.Invoke();
        close();
    }
    void close()
    {
        callback = null;
        callback2 = null;
        gameObject.SetActive(false);
    }
}

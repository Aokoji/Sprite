using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : UIBase
{
    public string PanelName;

    public virtual void init() 
    {
        registerEvent();
    }
    public virtual void registerEvent() { }
    public void initAfterLoading() { }
    public void OnExit() { }
}

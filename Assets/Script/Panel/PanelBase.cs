using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : UIBase
{
    public string PanelName;

    public virtual void init() { }
    public void initAfterLoading() { }
    public void OnExit() { }
}

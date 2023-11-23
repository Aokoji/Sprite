using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCallBack:MonoBehaviour
{
    public Action Callback = null;
    public void Mycallback()
    {
        Callback?.Invoke();
        Callback = null;
    }
}

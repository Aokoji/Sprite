using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimCallBack : MonoBehaviour
{
    //工具脚本  所有动画播放都会被自动添加
    public Action CallBack;
    public void animCallBack()
    {

        if (CallBack != null)
        {
            CallBack();
        }

    }
}

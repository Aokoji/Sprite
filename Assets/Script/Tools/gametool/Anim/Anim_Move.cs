using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Anim_Move : AnimNodeBase
{
    public void startPlay(Action callback=null)
    {
        runtime = 0;
        playallow = true;
        CallBack = callback;
    }
    Action CallBack;
    Vector3 targetPos;
    float time;
    float speed;
    float runtime;
    public void setData(GameObject target,float time,float speed)
    {
        targetPos = transform.localPosition + transform.InverseTransformPoint(target.transform.position);
        this.time = time;
        this.speed = speed;
        runtime = 0;
        playallow = false;
    }
    // Update is called once per frame
    private void Update()
    {
        if (playallow)
        {
            runtime += Time.deltaTime;
            transform.localPosition += (targetPos-transform.localPosition)*Time.deltaTime/(time - runtime);
            if (runtime >= time)
            {
                playallow = false;
                CallBack?.Invoke();
            }
        }
    }
}

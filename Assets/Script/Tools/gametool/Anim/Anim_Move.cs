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
    Vector3 startPos;
    Vector3 targetPos;
    Vector3 referPos;
    float time;
    int movetype;
    float runtime;
    /// <summary>
    /// 匀速
    /// </summary>
    /// <param name="target"></param>
    public void setData(GameObject target,float time,int type)
    {
        startPos = transform.localPosition;
        targetPos = startPos + transform.InverseTransformPoint(target.transform.position);
        referPos = Vector3.Lerp(startPos, targetPos, type / 10);
        movetype = type;
        this.time = time;
    }
    // Update is called once per frame
    float timegap;
    private void Update()
    {
        if (playallow)
        {
            runtime += Time.deltaTime;
            timegap = runtime / time;
            if (movetype == 5)
                transform.localPosition = Vector3.Lerp(startPos, targetPos, timegap);   //推荐插值运动
            else
                //新插值运动
                transform.localPosition = Vector3.Lerp(Vector3.Lerp(startPos, referPos, timegap), targetPos, timegap);
            //transform.localPosition += (targetPos-transform.localPosition)*Time.deltaTime/(time - runtime);


            if (runtime >= time)
            {
                playallow = false;
                transform.localPosition = targetPos;
                CallBack?.Invoke();
            }
        }
    }
}

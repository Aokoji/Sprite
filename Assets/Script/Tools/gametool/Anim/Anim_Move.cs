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

    Vector3 s_scale;
    Vector3 s_rotate;
    Vector3 t_scale;
    Vector3 t_rotate;
    float time;
    MoveType movetype;
    float runtime;
    /// <summary>
    /// 匀速
    /// </summary>
    /// <param name="target"></param>
    public void setData(Transform target,float time, MoveType type,Vector3 bezier)
    {
        startPos = transform.position;
        targetPos = target.position;
        t_scale = transform.localScale;
        t_rotate = transform.eulerAngles;
        movetype = type;
        if (movetype == MoveType.moveBezier)
            referPos = bezier;
        this.time = time;
    }
    public void setDataAll(Transform target, float time, MoveType type, Vector3 scale,Vector3 rotate)
    {
        startPos = transform.position;
        targetPos = target.position;
        movetype = type;
        s_scale = transform.localScale;
        s_rotate = transform.eulerAngles;
        t_scale = scale;
        t_rotate = rotate;
        anglegap = 0;
        if (Mathf.Abs(rotate.z - s_rotate.z) > 180)
        {
            if (rotate.z > s_rotate.z)
                anglegap = rotate.z - s_rotate.z + 360;
            else
                anglegap = rotate.z + 360 - s_rotate.z;
        }
        else
        {
            anglegap = rotate.z - s_rotate.z;
        }
        this.time = time;
    }


    float timegap;
    Vector3 rot;
    float anglegap;
    private void Update()
    {
        if (playallow)
        {
            runtime += Time.deltaTime;
            timegap = runtime / time;
            if (movetype == MoveType.moveto)
                transform.position = Vector3.Lerp(startPos, targetPos, timegap);   //推荐插值运动
            else if (movetype == MoveType.moveAll || movetype == MoveType.moveAll_FTS || movetype == MoveType.moveAll_STF)
            {
                if (movetype == MoveType.moveAll_STF)
                    timegap *= timegap;
                if (movetype == MoveType.moveAll_FTS)
                    timegap = Mathf.Sqrt(timegap);
                transform.position = Vector3.Lerp(startPos, targetPos, timegap);
                transform.localScale= Vector3.Lerp(s_scale, t_scale, timegap);
                float az = s_rotate.z + timegap * anglegap;
                if (az < 0) az += 360;
                if (az >= 360) az -= 360;
                rot.z = az;
                transform.eulerAngles = rot;
            }
            else if (movetype == MoveType.moveBezier)
            {
                transform.position = Vector3.Lerp(Vector3.Lerp(startPos, referPos, timegap), Vector3.Lerp(referPos, targetPos, timegap), timegap);
            }
            if (runtime >= time)
            {
                playallow = false;
                transform.position = targetPos;
                transform.localScale = t_scale;
                transform.eulerAngles = t_rotate;
                CallBack?.Invoke();
            }
        }
    }
}

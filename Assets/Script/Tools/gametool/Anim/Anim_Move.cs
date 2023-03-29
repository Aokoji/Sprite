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
    Quaternion s_rotate;
    Vector3 t_scale;
    Quaternion t_rotate;
    float time;
    MoveType movetype;
    float runtime;
    /// <summary>
    /// 匀速
    /// </summary>
    /// <param name="target"></param>
    public void setData(GameObject target,float time, MoveType type,Vector3 bezier)
    {
        startPos = transform.localPosition;
        targetPos = startPos + transform.InverseTransformPoint(target.transform.position);
        movetype = type;
        if (movetype == MoveType.moveBezier)
            referPos = bezier;
        this.time = time;
    }
    public void setDataAll(GameObject target, float time, MoveType type, Vector3 scale,Quaternion rotate)
    {
        startPos = transform.localPosition;
        targetPos = startPos + transform.InverseTransformPoint(target.transform.position);
        movetype = type;
        s_scale = transform.localScale;
        s_rotate = transform.localRotation;
        t_scale = scale;
        t_rotate = rotate;
        /*
        if (Mathf.Abs(rotate.z - transform.localEulerAngles.z) > 180)
        {
            if (rotate.z > transform.localEulerAngles.z)
            {
                anglegap = transform.localEulerAngles.z + 360 - rotate.z;
            }
            else
            {
                anglegap = transform.localEulerAngles.z - rotate.z - 360;
            }
        }*/
        Debug.Log(anglegap);
        this.time = time;
    }
    // Update is called once per frame
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
                transform.localPosition = Vector3.Lerp(startPos, targetPos, timegap);   //推荐插值运动
            else if (movetype == MoveType.moveAll)
            {
                transform.localPosition = Vector3.Lerp(startPos, targetPos, timegap);
                //transform.localRotation = Quaternion.Euler(Vector3.Lerp(s_rotate, t_rotate, timegap));
                transform.localScale= Vector3.Lerp(s_scale, t_scale, timegap);
                
                transform.Rotate(new Vector3(0,0,45), Space.World);
            }
            else if (movetype == MoveType.moveBezier)
            {
                transform.localPosition = Vector3.Lerp(Vector3.Lerp(startPos, referPos, timegap), Vector3.Lerp(referPos, targetPos, timegap), timegap);
            }
                //新插值运动
                
            //transform.localPosition += (targetPos-transform.localPosition)*Time.deltaTime/(time - runtime);


            if (runtime >= time)
            {
                playallow = false;
                transform.localPosition = targetPos;
                transform.localScale = t_scale;
                transform.localRotation = t_rotate;
                CallBack?.Invoke();
            }
        }
    }
}

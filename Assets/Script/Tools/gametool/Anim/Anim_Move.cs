using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Move : MonoBehaviour
{
    private bool playallow;
    public void starPlay()
    {
        runtime = 0;
        playallow = true;
        Debug.Log("===");
    }
    Vector3 targetPos;
    float time;
    float speed;
    Vector3 gap;
    float runtime;
    public void setData(GameObject target,float time,float speed)
    {
        targetPos = transform.InverseTransformPoint(target.transform.position);
        this.time = time;
        this.speed = speed;
        runtime = 0;
        gap = Time.deltaTime/time * targetPos;
        playallow = false;
        Debug.Log(gap+"===");
    }
    // Update is called once per frame
    private void Update()
    {
        if (playallow)
        {
            Debug.Log(gap);
            runtime += Time.deltaTime;
            transform.position += gap;
            if (runtime >= time)
                playallow = false;
        }
    }
}

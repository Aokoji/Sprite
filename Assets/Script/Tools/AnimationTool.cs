using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationTool
{
    #region 第一套动画方案 animator
    /// <summary>
    /// 外部调用 播放组件动画
    /// </summary>
    /// <param name="obj">组件gameobj</param>
    /// <param name="aniName">动画名</param>
    /// <param name="loop">循环</param>
    /// <param name="callBack">回调</param>
    public static void playAnimation(GameObject obj, string aniName, bool loop, Action callBack)
    {
        Animator anim = obj.GetComponent<Animator>();
        float time = getAnimTime(anim, aniName);
        if (time == 0)
        {
            Debug.LogError("animation  time  is  null  !");
            return;
        }
        obj.SetActive(true);
        anim.Play(aniName, 0, 0f);
        void action()
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            //if (obj.tag == "Player") Debug.Log(aniName+ info.normalizedTime);     //动画错误监测
            if (obj.activeSelf  &&  info.IsName(aniName) && info.normalizedTime >= 0.8f)
            {
                if (!loop) anim.StopPlayback();
                callBack();
            }
        }
        PubTool.Instance.laterDo(time, action);
    }
    public static void playAnimation(GameObject obj, string aniName, bool loop=false)
    {
        Animator anim = obj.GetComponent<Animator>();
        float time = getAnimTime(anim, aniName);
        if (time == 0)
        {
            Debug.LogError("animation  time  is  null  !");
            return;
        }
        obj.SetActive(true);
        anim.Play(aniName, 0, 0f);
    }

    public static void playAnimation(GameObject obj)
    {
        Animator anim = obj.GetComponent<Animator>();
        obj.SetActive(true);
        anim.Play("default", 0, 0f);
    }
    #endregion

    //  第二套动画方案
    #region
    public static void playAnim(GameObject obj,string sname)
    {
        Animation anim = obj.GetComponent<Animation>();
        AnimationClip clip = anim.GetClip(sname);
        if (clip == null)
        {
            Debug.LogError("动画不存在！" + sname);
            return;
        }
        if (obj.GetComponent<AnimCallBack>() == null)
            obj.AddComponent<AnimCallBack>();
        obj.GetComponent<AnimCallBack>().CallBack = null;
        clip.events = null;
        anim.Rewind();
        anim.gameObject.SetActive(true);
        if (!string.IsNullOrEmpty(sname))
        {
            anim.Play(sname);
            anim[sname].time = 0;
            anim.Sample();
        }
        else
        {
            Debug.Log("播放了默认动画！");
            anim.Play();
        }
    }

    public static void playAnim(GameObject obj, string sname, Action callback)
    {
        Animation anim = obj.GetComponent<Animation>();
        AnimationClip clip = anim.GetClip(sname);
        if (clip == null)
        {
            Debug.LogError("动画不存在！"+sname);
            return;
        }
        if (obj.GetComponent<AnimCallBack>() == null)
            obj.AddComponent<AnimCallBack>();
        clip.events=null;
        AnimationEvent eve = new AnimationEvent();
        eve.functionName = "animCallBack";
        eve.time = anim[sname].length;
        clip.AddEvent(eve);
        obj.GetComponent<AnimCallBack>().CallBack=callback;
        anim.Rewind();
        anim.gameObject.SetActive(true);
        if (!string.IsNullOrEmpty(sname))
        {
            anim.Play(sname);
            anim[sname].time = 0;
            anim.Sample();
        }
        else
        {
            Debug.Log("播放了默认动画！");
            anim.Play();
        }
    }

    #endregion

    //工具  获取动画时长
    private static float getAnimTime(Animator anim, string aniName)
    {
        if (null == anim)
        {
            Debug.LogError("animation  is  null  !");
            return 0;
        }
        var clips = anim.runtimeAnimatorController.animationClips;
        if (null == clips || clips.Length <= 0)
        {
            Debug.LogError("animation  clips  is  wrong  !");
            return 0;
        }
        AnimationClip clip;
        for (int i = 0, len = clips.Length; i < len; ++i)
        {
            clip = clips[i];
            if (null != clip && clip.name == aniName)
                return clip.length;
        }
        return 0;
    }
}

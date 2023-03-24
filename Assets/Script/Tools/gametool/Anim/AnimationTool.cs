using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationTool
{
    /// <summary>
    /// 外部调用 播放组件动画
    /// </summary>
    /// <param name="obj">组件gameobj</param>
    /// <param name="aniName">动画名</param>
    /// <param name="loop">循环</param>
    /// <param name="callBack">回调</param>
    public static void playAnimatior(GameObject obj, string aniName, bool loop, Action callBack)
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
        RunSingel.Instance.laterDo(time, action);
    }
    public static void playAnimation(GameObject obj, string aniName, bool loop)
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
    //++++动画需要单独测试  测试上一个结束重复播放和播下一个是否流畅

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

    public static void playAnimation(GameObject obj,string aniname,bool isloop=false,Action callback=null)
    {
        Animation anim = obj.GetComponent<Animation>();
        if (anim = null)
        {
            Debug.LogError("animation component is null,will play animName =" + aniname);
            return;
        }
        var script = obj.GetComponent<AnimCallBack>();
        if (null == script)
            script = obj.AddComponent<AnimCallBack>();
        script.Callback = callback;
        AnimationClip clip = anim.GetClip(aniname);
        AnimationEvent evt = new AnimationEvent();
        evt.functionName = "Mycallback";
        evt.time = clip.length;
        clip.events = null;
        clip.AddEvent(evt);
        anim.clip = clip;
        anim.Play();
    }

    public static void moveTo(GameObject obj, GameObject target, float time, int speed = 1)
    {
        RunSingel.Instance.moveTo(obj, target, time, speed);
    }

}

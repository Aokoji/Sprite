using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PanelBase : UIBase
{
    public string PanelName;
    protected object[] message;

    public void init(object[] obj)
    {
        message = obj;
        registerEvent();
        init();
        initAnimType();
    }
    //默认打开关闭动画
    public virtual void initAnimType()
    {
        var anim = GetComponent<Animation>();
        if (anim == null)
        {
            anim = gameObject.AddComponent<Animation>();
            AnimationClip clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Hide_Panel2");
            anim.AddClip(clip, "Hide_Panel2");
            clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Show_Panel2");
            anim.AddClip(clip, "Show_Panel2");
        }
        AnimationTool.playAnimation(gameObject, "Show_Panel2", false, afterAnimComplete);
        anim.clip = anim.GetClip("Show_Panel2");
        anim.Play();
    }
    public virtual void init() { }
    public virtual void registerEvent() { }
    public virtual void reshow() { }
    public void initAfterLoading() { }
    public virtual void afterAnimComplete() { }
    private void Update() { OnUpdate(); }
    public virtual void OnUpdate() { }
    public void OnExit() { }
    public virtual void Dispose() {
        AnimationTool.playAnimation(gameObject, "Hide_Panel2", false, () => { Destroy(gameObject); });
    }
}

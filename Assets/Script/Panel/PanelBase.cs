using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : UIBase
{
    public string PanelName;

    public void init(Action complete) 
    {
        var anim = GetComponent<Animation>();
        if (anim== null)
        {
            anim = gameObject.AddComponent<Animation>();
            AnimationClip clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Hide_Panel2");
            anim.AddClip(clip, "Hide_Panel2");
            clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Show_Panel2");
            anim.AddClip(clip, "Show_Panel2");
        }
        AnimationTool.playAnimation(gameObject, "Show_Panel2", false, complete);
        anim.clip = anim.GetClip("Show_Panel2");
        anim.Play();
        registerEvent();
        init();
    }
    public virtual void init() { }
    public virtual void registerEvent() { }
    public virtual void reshow() { }
    public void initAfterLoading() { }
    public void OnExit() { }
    public virtual void Dispose() {
        AnimationTool.playAnimation(gameObject, "Hide_Panel2", false, () => { Destroy(gameObject); });

    }
}

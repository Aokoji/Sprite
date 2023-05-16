using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTopBase : PanelBase
{
    public override void init(Action complete)
    {
        var anim = GetComponent<Animation>();
        if (anim == null)
        {
            anim = gameObject.AddComponent<Animation>();
            AnimationClip clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Hide_Panel");
            anim.AddClip(clip, "Hide_Panel");
            clip = AssetManager.loadAsset<AnimationClip>("Art/anim/tips/Show_Panel");
            anim.AddClip(clip, "Show_Panel");
        }
        AnimationTool.playAnimation(gameObject, "Show_Panel", false, complete);
        anim.clip = anim.GetClip("Show_Panel");
        anim.Play();
        registerEvent();
        init();
    }
    public override void Dispose()
    {
        AnimationTool.playAnimation(gameObject, "Hide_Panel", false, () => { Destroy(gameObject); });

    }
}

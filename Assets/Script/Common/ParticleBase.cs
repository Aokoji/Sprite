using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ParticleBase:MonoBehaviour
{
    public Text tx;

    public void play(string context, string aniname, Action callback)
    {
        if (string.IsNullOrEmpty(context) || context == "") ;
        else
            tx.text = context;
        AnimationTool.playAnimation(gameObject, aniname, false, callback);
    }
    public void play(string aniname, Action callback)
    {
        if (tx != null)
            tx.gameObject.SetActive(false);
        AnimationTool.playAnimation(gameObject, aniname, false, callback);
    }
}

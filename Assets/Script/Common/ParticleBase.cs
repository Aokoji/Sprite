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
        tx.text = context;
        AnimationTool.playAnimation(gameObject, aniname, false, callback);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ParticleBase:MonoBehaviour
{
    public Text txbg;
    public Text tx;

    public void play(string context,Action callback)
    {
        txbg.text = context;
        tx.text = context;
        AnimationTool.playAnimator(gameObject, "hitPar", false, callback);
    }
}

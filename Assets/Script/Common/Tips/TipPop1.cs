using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipPop1 : TipsBase
{
    public override void play()
    {
        AnimationTool.playAnimation(gameObject, "showtip1", false, () => { callback?.Invoke(); });
    }
}

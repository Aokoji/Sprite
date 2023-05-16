using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPop3 : TipsBase
{
    public override void play()
    {
        AnimationTool.playAnimation(gameObject, "showtip3", false, () => { gameObject.SetActive(false); });
    }
}

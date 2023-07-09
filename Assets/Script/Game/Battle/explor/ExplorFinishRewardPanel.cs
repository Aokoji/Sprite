using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorFinishRewardPanel : PanelTopBase
{
    public Text title;
    public Button okbtn;
    public Image rewardIcon;
    public Text rewardContext;

    public override void init()
    {
        base.init();
        var mess = message[0] as ExplorMovingPanel.rankReward;
        if (mess.stype == explorIcon.exitBox)
        {

        }
    }
}

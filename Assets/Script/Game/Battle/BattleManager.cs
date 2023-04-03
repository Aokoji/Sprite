using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : CSingel<BattleManager>
{
    BattleControl ctrl;

    public void init()
    {
        ctrl=new BattleControl();
        refreshBattleData();
        loadDataOnMain();   //+++test
    }
    public void loadDataOnMain()
    {
        
    }

    public void EnterBattle()
    {
        refreshBattleData();
        Loadingbattle();
    }

    private void refreshBattleData()
    {

    }



    public void Loadingbattle()
    {
        loadScene();
    }
    private void Loadcomplete()
    {
        //播放开始动画 或者战斗信息
        ctrl.StartRound();
    }
    private void loadScene()
    {
        RunSingel.Instance.runTimer(loadSceneTimer());
    }
    IEnumerator loadSceneTimer()
    {
        ctrl.newbattle();   //加载信息
        while(!ctrl.loadSuccess)
            yield return null;
        Loadcomplete();
    }
}

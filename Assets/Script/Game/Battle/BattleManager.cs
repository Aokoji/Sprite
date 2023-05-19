using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : CSingel<BattleManager>
{
    BattleControl ctrl;         //目前未做销毁    +++
    public int battleEnemyID;

    public void init()
    {
        refreshBattleData();
    }

    public void EnterBattle()
    {
        ctrl = new BattleControl();
        refreshBattleData();
        ctrl.newbattle();
    }

    private void refreshBattleData()
    {

    }
}

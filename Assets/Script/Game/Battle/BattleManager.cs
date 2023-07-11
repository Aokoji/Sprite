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

    public void EnterBattle(int enemy,explorIcon stype=explorIcon.battle)
    {
        ctrl = new BattleControl();
        refreshBattleData();
        ctrl.newbattle(enemy);
    }

    private void refreshBattleData()
    {

    }

    public void endreward(int id)
    {

    }
}

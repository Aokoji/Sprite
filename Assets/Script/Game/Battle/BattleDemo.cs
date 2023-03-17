using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDemo : MonoBehaviour
{
    void Start()
    {
        BattleManager.Instance.init();
        loadDataTest();
        BattleManager.Instance.EnterBattle();
    }

    private void loadDataTest()
    {

    }

    
}

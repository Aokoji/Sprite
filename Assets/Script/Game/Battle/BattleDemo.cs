using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDemo : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.initManager();
        loadDataTest();
        BattleManager.Instance.EnterBattle();
    }

    private void loadDataTest()
    {

    }

    private void Update()
    {
        
    }
}

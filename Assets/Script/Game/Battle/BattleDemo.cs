using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDemo : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.initManager();
        loadDataTest();
    }

    private void loadDataTest()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            BattleManager.Instance.EnterBattle();
    }
}

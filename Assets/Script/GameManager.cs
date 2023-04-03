using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class GameManager : CSingel<GameManager>
{
    public void initManager()
    {
        RunSingel.Instance.runTimer(managerInit());
    }
    IEnumerator managerInit()
    {
        PanelManager.Instance.init();
        TableManager.Instance.init();
        while (!TableManager.Instance.loadsuccess)
            yield return null;
        PlayerManager.Instance.init();
        while (!PlayerManager.Instance.loadsuccess)
            yield return null;
        BattleManager.Instance.init();
    }
    
}

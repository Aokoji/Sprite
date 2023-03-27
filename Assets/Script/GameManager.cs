using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using customEvent;

public class GameManager : CSingel<GameManager>
{
    public void initManager()
    {
        PanelManager.Instance.init();
        PlayerManager.Instance.init();
        BattleManager.Instance.init();
    }
    
    
}

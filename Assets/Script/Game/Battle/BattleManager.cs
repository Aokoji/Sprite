using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : CSingel<BattleManager>
{
    BattleControl ctrl;
    //所有卡片数据
    private Dictionary<int, CardData> cardDataDic = new Dictionary<int, CardData>();
    public Dictionary<int, CardData> CardDataDic { get { return cardDataDic; } }

    public CardData getOneCard(int id)
    {
        if (CardDataDic.ContainsKey(id))
            return CardDataDic[id];
        else
            return null;
    }

    public void init()
    {
        ctrl=new BattleControl();
        refreshBattleData();
        loadDataOnMain();   //+++test
    }
    string DATA_PATH = "Assets/config/DataTestCard.csv";
    public void loadDataOnMain()
    {
        cardDataDic = LoadDataAdapter.loadDataCard(DATA_PATH);
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

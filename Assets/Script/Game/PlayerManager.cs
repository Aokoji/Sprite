using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : CSingel<PlayerManager>
{
    //PlayerAsset playerAsset;
    PlayerData playerdata;  //元数据
    //  ------------    解析数据
    public SpriteData cursprite;

    public bool loadsuccess;
    public void init()
    {
        loadsuccess = false;
        injectPlayer();
    }

    public void injectPlayer()
    {//暂定为读asset文件
        loadTestCardData();
        //初始化sprite
        playerdata.sprites.ForEach(item => { if (item.id == playerdata.curSprite) cursprite = item; });
        loadsuccess = true;
    }
    private void loadDefaultData(int id)
    {

    }
    
    //private string CARD_TEST_PATH = "config/assetdata/playersave";
    public void loadTestCardData()
    {
        var data= AssetManager.loadJson<PlayerData>(S_SaverNames.pdata.ToString());
        if (data!=null)
        {
            //playerAsset = AssetManager.loadAsset<PlayerAsset>(CARD_TEST_PATH);
            playerdata = data;
        }
        else
        {
            //playerAsset = ScriptableObject.CreateInstance<PlayerAsset>();
            playerdata = new PlayerData();
            playerdata.initdata();
            //playerAsset.playdata = playerdata;
            //AssetManager.saveAsset(playerAsset, CARD_TEST_PATH + ".asset");
            savePlayerData();
        }
    }
    public void savePlayerData()
    {
        AssetManager.saveJson(S_SaverNames.pdata.ToString(), playerdata); 
    }

    public List<int> getPlayerCards()
    {
        return playerdata.playerCards;
    }
    public void setPlayerCards(List<int> cards)
    {
        playerdata.playerCards= cards;
        savePlayerData();
    }
}

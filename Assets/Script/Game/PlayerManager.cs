using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : CSingel<PlayerManager>
{
    PlayerAsset playerAsset;
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
    
    private string CARD_TEST_PATH = "Assets/config/assetdata/playersave";
    public void loadTestCardData()
    {
        if (File.Exists(CARD_TEST_PATH + ".asset"))
        {
            playerAsset = AssetManager.loadAssetFile<PlayerAsset>(CARD_TEST_PATH);
            playerdata = playerAsset.playdata;
        }
        else
        {
            playerAsset = ScriptableObject.CreateInstance<PlayerAsset>();
            playerdata = new PlayerData();
            playerdata.initdata();
            playerdata.battleTest();
            playerAsset.playdata = playerdata;
            AssetManager.saveAsset(playerAsset, CARD_TEST_PATH + ".asset");
        }
    }

    public List<int> getPlayerCards()
    {
        return playerdata.playerCards;
    }
}

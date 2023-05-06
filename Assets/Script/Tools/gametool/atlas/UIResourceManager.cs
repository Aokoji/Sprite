using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

public class UIResourceManager : CSingel<UIResourceManager>
{
    private Dictionary<string, SpriteAtlas> uiAtlasDic = new Dictionary<string, SpriteAtlas>();
    string ATLAS_PATH = "ui/atlas/";

    public Sprite GetSpriteAtlas(string atlasname,string spritename)
    {
        if (!uiAtlasDic.ContainsKey(atlasname))
        {
            var altas = AssetManager.loadAsset<SpriteAtlas>(ATLAS_PATH+ atlasname);
            if (altas == null) Debug.LogError("altas name 输入错误!");
            uiAtlasDic.Add(atlasname, altas);
        }
        return uiAtlasDic[atlasname].GetSprite(spritename);
    }
    private string getspritePath(string atlasname)
    {
        return "";  //+++每一个图集都应该有路径记录
    }
}

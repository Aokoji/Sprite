using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

public class UIResourceManager : CSingel<UIResourceManager>
{
    private Dictionary<string, SpriteAtlas> uiAtlasDic = new Dictionary<string, SpriteAtlas>();

    public Sprite GetSpriteAtlas(string atlasname,string spritename)
    {
        if (!uiAtlasDic.ContainsKey(atlasname))
            uiAtlasDic.Add(atlasname, loadAtlasResource(atlasname, getspritePath(atlasname)));
        return uiAtlasDic[atlasname].GetSprite(spritename);
    }
    private string getspritePath(string atlasname)
    {
        return "";  //+++每一个图集都应该有路径记录
    }
    private SpriteAtlas loadAtlasResource(string sname, string path)
    {
        return AssetManager.loadAsset<SpriteAtlas>(sname, path, ".spriteatlas");
    }
}

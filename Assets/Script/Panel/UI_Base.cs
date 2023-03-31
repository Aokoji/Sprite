using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected string FullPath;  //ui路径
    protected Sprite GetSprite(string atlas, string sname)
    {
        return UIResourceManager.Instance.GetSpriteAtlas(atlas, sname);
    }
}

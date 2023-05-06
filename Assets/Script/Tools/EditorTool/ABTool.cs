using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class ABTool : Editor
{
    [MenuItem("datatool/AssetBundlePackager")]
    public static void packageAB()
    {
        string dirpath = "StreamingAssets";
        if (!Directory.Exists(dirpath))
            Directory.CreateDirectory(dirpath);
        //      路径，压缩算法，设备androiid/ios
        BuildPipeline.BuildAssetBundles(dirpath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        Debug.Log("资源打包完成");
    }
}
#endif
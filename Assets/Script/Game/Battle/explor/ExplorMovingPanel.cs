using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorMovingPanel : PanelBase
{
    public GameObject startPos;
    public GameObject endPos;

    public GameObject LineClone;
    public GameObject pointClone;

    List<GameObject> points;
    List<GameObject> lines;     //统一销毁

    public override void init()
    {
        base.init();
        points = new List<GameObject>();
        lines = new List<GameObject>();
    }
    public override void registerEvent()
    {
        base.registerEvent();
    }

    #region calculate
    //棋盘        3x3 4x4 5x4 5x5 5x6 6x6 
    Vector2 curPos;
    Dictionary<Vector2, int> trackPos;
    void initCalculate()
    {
        curPos = Vector2.zero;
        trackPos = new Dictionary<Vector2, int>();
    }
    void moveToNext()
    {
        trackPos[curPos] = 1;
        var vec = findNextPoint();
    }
    Vector2 findNextPoint()
    {
        Vector2 result=curPos;
        for(int i = 0; i < 8; i++)
        {
            //budui
        }
        return result;
    }
    #endregion
    public override void Dispose()
    {
        lines.ForEach(item => { Destroy(item); });
        points.ForEach(item => { Destroy(item); });
        base.Dispose();
    }
}

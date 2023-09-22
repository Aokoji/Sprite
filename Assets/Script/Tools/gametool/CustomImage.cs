using UnityEngine;
using UnityEngine.UI;
//自定义多边形交互（碎片）图片
public class CustomImage : Image
{
    private PolygonCollider2D _polygon;
    public PolygonCollider2D polygon
    {
        get {
            if (_polygon == null)
            {
                _polygon = GetComponent<PolygonCollider2D>();
            }
            return _polygon;
        }
    }
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 point;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);
        return polygon.OverlapPoint(point);
    }
}

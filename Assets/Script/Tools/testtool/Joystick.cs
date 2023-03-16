using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick :MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject item; //摇杆
    public Camera cam;      //ui屏幕相机（可以手动获取
    private Vector2 savepos;
    private float radius = 1;
    public static float axisX;
    public static float axisY;

    private void Start()
    {
        savepos = item.transform.position;  //记录初始位置
        radius = 1;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        axisX = 0;
        axisY = 0;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = cam.ScreenToWorldPoint(eventData.position);
        axisX = Mathf.Min(Mathf.Max(pos.x - savepos.x, -1), 1);
        axisY = Mathf.Min(Mathf.Max(pos.y - savepos.y, -1), 1);
        if (Vector2.Distance(pos,savepos) > radius)
            item.transform.position = savepos+(pos - savepos).normalized*radius;
        else
            item.transform.position = pos;
    }
}

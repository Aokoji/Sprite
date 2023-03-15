using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject item;
    public Camera cam;
    private Vector2 savepos;
    private float radius;
    private void Start()
    {
        savepos = item.transform.position;
        radius = 1;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        actorControl.Instance.setAxis(0);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = cam.ScreenToWorldPoint(eventData.position);
        actorControl.Instance.setAxis(Mathf.Min(Mathf.Max(pos.x - savepos.x,-1),1));
        if (Vector2.Distance(pos,savepos) > radius)
            item.transform.position = savepos+(pos - savepos).normalized*radius;
        else
            item.transform.position = pos;
    }
}

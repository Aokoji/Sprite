using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAdapter : MonoBehaviour
{
    public static float width;
    public static float heigh;
    public static Vector2 peix = new Vector2(1280, 720);
    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width;
        heigh = Screen.height;
        float widthscale = width / peix.x;
        float heighscale = heigh / peix.y;
        transform.localScale = Vector2.one * Mathf.Min(widthscale,heighscale);
    }

}

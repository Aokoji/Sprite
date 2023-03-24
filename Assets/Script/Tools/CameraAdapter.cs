using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//不用了
public class CameraAdapter : MonoBehaviour
{
    void Start()
    {
        screenX = Screen.width;
        screenY = Screen.height;
        adapteCamera();
    }

    public Vector2 cameraSize;
    float screenX;
    float screenY;

    public void adapteCamera()
    {
        float size = 5 * (cameraSize.x / cameraSize.y) / (screenX / screenY);
        GetComponent<Camera>().orthographicSize = size;
    }
}

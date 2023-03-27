using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public bool isplay;

    public void startplay()
    {
        isplay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isplay)
        {
            Debug.Log("===22222====");
            isplay = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject obj = new GameObject();
            var sp = obj.AddComponent<test>();
            sp.startplay();
        }
    }
}

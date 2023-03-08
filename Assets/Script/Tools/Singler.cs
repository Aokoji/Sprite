using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singler<T>:MonoBehaviour where T:class , new()
{
    protected static T instance;
    public static T Instance
    {
        get {
            if (null == instance)
            {
                instance = new T();
            }
            return instance;
        }
    }
}

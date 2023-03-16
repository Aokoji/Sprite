using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSingel<T> where T :class
{
    private static T instance;
    private static object initlock = new object();
    public static T Instance
    {
        get {
            if (instance == null)
                createInstance();
            return instance;
        }
    }

    private static void createInstance()
    {
        lock (initlock)
        {
            if (instance == null)
            {
                Type t = typeof(T);
                instance = (T)Activator.CreateInstance(t, true);
            }
        }
    }
}

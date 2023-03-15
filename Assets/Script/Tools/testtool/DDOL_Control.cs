using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DDOL_Control<T> : MonoBehaviour where T : DDOL_Control<T>
{
    protected static T instance = null;
    public static T Instance
    {
        get {
            if (null == instance)
            {
                GameObject ctrl = GameManager.Instance.gameObject;
                instance = ctrl.GetComponent<T>();
                if (null == instance)
                {
                    instance = ctrl.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}

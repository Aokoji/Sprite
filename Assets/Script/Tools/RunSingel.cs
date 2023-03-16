using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSingel : DDOL_Control<RunSingel>
{
    public void runTimer(IEnumerator obj)
    {
        StartCoroutine(obj);
    }
}

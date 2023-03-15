using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeClearedTest : MonoBehaviour
{
    private bool isallow;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isallow)
            {
                isallow = true;
                actorControl.Instance.enterDialog();
                FrameControl.Instance.beCleared();
            }
        }
    }

}

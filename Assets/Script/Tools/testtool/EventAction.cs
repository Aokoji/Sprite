using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventAction : DDOL_Control<EventAction>
{
    public delegate void hitEvent(ActorMessage actor);
    public event hitEvent sendHitEvent = new hitEvent(nullfunction);
    public void hitAction(ActorMessage actor)
    {
        sendHitEvent(actor);
    }

    private static void nullfunction(ActorMessage actor) { }
}

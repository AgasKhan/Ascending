using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteActivate_Interactuable : Interactuable_LogicActive
{
    // Start is called before the first frame update

    [SerializeReference]
    LogicActive[] remote;

    public bool active = false;

    public override void Activate()
    {
        foreach (var item in remote)
        {
            item.Activate(active);
        }

        foreach (var item in animator)
        {
            item.Play(active ? "On" : "Off");
        }

        active = !active;
    }
}

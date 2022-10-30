using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteActivate_Interactuable : Interactuable_LogicActive
{
    // Start is called before the first frame update

    [SerializeReference]
    LogicActive[] remote;

    public override void Activate()
    {
        foreach (var item in remote)
        {
            item.Activate();
        }
    }
}

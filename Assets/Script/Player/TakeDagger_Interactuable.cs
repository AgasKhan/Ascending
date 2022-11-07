using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDagger_Interactuable : Interactuable_LogicActive
{

    [SerializeField]
    Dagger_Proyectile me;

    private void Awake()
    {
        me= GetComponentInParent<Dagger_Proyectile>();
    }

    public override void Activate()
    {
        GameManager.player.dagger = me;
        //diseable = true;

        DebugPrint.Log("Se ha accionado la: " + name.RichText("color", "green"));
    }
}

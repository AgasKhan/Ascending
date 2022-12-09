using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Proyectile : Proyectile
{
    protected override void OnEnter(Collider other)
    {
        base.OnEnter(other);

        CasterObject();

        gameObject.SetActive(false);
    }
}

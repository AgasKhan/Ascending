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

    protected override void OnDamage(IOnProyectileEnter damaged)
    {
        base.OnDamage(damaged);

        if (OnDamaged(damaged, out Character ch))
        {
            AplicateDebuff(ch);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        OnEnter(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Proyectile : Proyectile
{

    void Active(Collider other)
    {
        Damage(other);

        CasterObject();

        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        Active(other);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerDetection : MonoBehaviour, IOnProyectileEnter, IOnProyectileExit
{
    [SerializeField]
    LogicActive[] active;

    [SerializeField]
    LogicActive[] deactive;

    public void ProyectileEnter(Damage damage)
    {
        if (!(damage.proyectile is Dagger_Proyectile))
            return;

        foreach (var item in active)
        {
            item.Activate();
        }

    }

    public void ProyectileExit()
    {
        foreach (var item in deactive)
        {
            item.Activate();
        }
    }

   
}

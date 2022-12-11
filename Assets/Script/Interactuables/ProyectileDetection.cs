using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileDetection : MonoBehaviour, IOnProyectileEnter, IOnProyectileExit
{
    [SerializeField]
    LogicActive[] active;

    [SerializeField]
    LogicActive[] deactive;

    public virtual void ProyectileEnter(Damage damage)
    {
        foreach (var item in active)
        {
            item.Activate();
        }
    }

    public virtual void ProyectileExit()
    {
        foreach (var item in deactive)
        {
            item.Activate();
        }
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileDetection : MonoBehaviour, IOnProyectileEnter, IOnProyectileExit
{
    [SerializeField]
    LogicActive[] active;

    [SerializeField]
    LogicActive[] deactive;

    public bool enter;

    public bool exit;

    public virtual void ProyectileEnter(Damage damage)
    {
        foreach (var item in active)
        {
            item.Activate(enter);
        }
    }

    public virtual void ProyectileExit()
    {
        foreach (var item in deactive)
        {
            item.Activate(exit);
        }
    }

   
}

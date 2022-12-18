using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrToPatrol : MoveTr, IPatrolReturn
{

    //referencia del script de patrulla
    public Patrol patrol;

    /// <summary>
    /// distancia minima para ir al siguiente objetivo
    /// </summary>
    [SerializeField]
    protected float _distance;

    public Patrol PatrolReturn()
    {
        return patrol;
    }

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
        
        MyUpdates += MyUpdate;
        
    }

    void MyAwake()
    {
        
        patrol.Start(this);

        desactivateDesaceleration = true;
    }

    void MyUpdate()
    {
        patrol.MinimalChck(_distance);

        MoveTo(patrol.Distance());
    }

}

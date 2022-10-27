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
    float _distance;

    public Patrol PatrolReturn()
    {
        return patrol;
    }

    protected override void MyAwake()
    {
        base.MyAwake();

        patrol.Start(this);

        desactivateDesaceleration = true;
    }

    protected override void MyUpdate()
    {
        base.MyUpdate();

        patrol.MinimalChck(_distance);

        MoveTo(patrol.Distance());
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRotTrToPatrol : MoveAndRotTr, IPatrolReturn
{
    /// <summary>
    /// minimo angulo antes de rotacion
    /// </summary>
    public float minAngleRot;

    /// <summary>
    /// referencia del script de patrulla
    /// </summary>
    public Patrol patrol;

    /// <summary>
    /// distancia minima para ir al siguiente objetivo
    /// </summary>
    [SerializeField]
    float _distance;

    Vector3 actualDist;

    Vector3 nextDist;

    public Patrol PatrolReturn()
    {
        return patrol;
    }

    protected override void MyAwake()
    {
        base.MyAwake();

        patrol.Start(this);

        desactivateDesaceleration = true;

        actualDist = patrol.Distance();

        nextDist = actualDist;
    }

    protected override void MyUpdate()
    {
        
        if(patrol.MinimalChck(_distance, false))
        { 
            nextDist = patrol.Distance(patrol.NextPoint(out bool b));
        }

        float angle = Utilitys.AngleOffAroundAxis(nextDist, Vector3.forward, Vector3.up, false);

        //print("angulo1: " + angle);

        float angle2 = Utilitys.AngleOffAroundAxis(nextDist, transform.forward, Vector3.up);

        //print("angulo 2: " + Mathf.Abs(angle2) + " < " + minAngleRot);

        if (Mathf.Abs(angle2) < minAngleRot)
        {
            
            patrol.MinimalChck(_distance, true);
        }
        else
            RotationY(angle);

        actualDist = patrol.Distance();

        MoveTo(actualDist);

        base.MyUpdate();

    }
}

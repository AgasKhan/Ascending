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

        actualDist = patrol.Distance();

        nextDist = actualDist;

        patrol.fsmPatrol.OnMove += ()=> patrol.MinimalChck(_distance);

        patrol.fsmPatrol.OnWait += MyUpdateWait;

        patrol.fsmPatrol.OnStartWait += OnStartWait;

        MyUpdates += patrol.fsmPatrol.UpdateState;
    }

    void MyUpdate()
    {
        actualDist = patrol.Distance();
        MoveTo(actualDist);
    }

    void OnStartWait()
    {
        bool aux = patrol.reverse;
        nextDist = patrol.Distance(patrol.NextPoint(ref aux));
    }

    void MyUpdateWait()
    {
        float angle = Utilitys.AngleOffAroundAxis(nextDist, Vector3.forward, Vector3.up, false);

        float angle2 = Utilitys.AngleOffAroundAxis(nextDist, transform.forward, Vector3.up);

        if (Mathf.Abs(angle2) >= minAngleRot)
        {
            RotationY(angle);
            ((Wait)patrol.fsmPatrol.wait).timer.Reset();
        }
    }

   
}

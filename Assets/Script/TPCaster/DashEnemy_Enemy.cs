using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy_Enemy : Enemy_Character
{

    [SerializeField] private float _flashDistance;

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
        
        MyUpdates += MyUpdate;

    }

    void MyAwake()
    {
        animator.functions.AddRange
        (
           new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               {"dash", Dash}
               
           }
        );
    }

    void MyUpdate()
    {
        if (playerDetF && attackDelay.Chck() && (scopedPoint-transform.position).sqrMagnitude <= 25)
        {
            attackDelay.Reset();
            movement.DashLocal(Vector3.forward);            
        }
    }
}

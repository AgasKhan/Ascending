using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy_Enemy : Enemy_Character
{
    //[SerializeField] private float _flashDistance;

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
        
        MyUpdates += MyUpdate;

    }

    public override void AttackSound()
    {
        throw new System.NotImplementedException();
    }

    public override void AuxiliarSound()
    {
        throw new System.NotImplementedException();
    }

    public override void DeathSound()
    {
        throw new System.NotImplementedException();
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
        if (attackDelay.Chck() && (scopedPoint-transform.position).sqrMagnitude <= 25 && scoped.gameObject.CompareOneTags("Player", "Traspasable"))
        {
            attackDelay.Reset();
            movement.DashLocal(Vector3.forward);            
        }
    }

    public override void PowerSound()
    {
        throw new System.NotImplementedException();
    }

    public override void DashSound()
    {
        audioM.Play("EnemyDash");
    }

    public override void WoRSoundLeft()
    {
        throw new System.NotImplementedException();
    }

    public override void WoRSoundRight()
    {
        throw new System.NotImplementedException();
    }
}

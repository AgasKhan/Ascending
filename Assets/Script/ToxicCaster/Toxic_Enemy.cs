using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxic_Enemy : Enemy_Character
{
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
               {"power", ActivePower},
               {"shoot", Shoot}
              
           }
        );
    }

    
    void MyUpdate()
    {
        
        if (playerDetF && attackDelay.Chck())
        {
            attackDelay.Reset();

            animator.Attack();

            //Shoot();
        }
        else if (powerObjectSpawn.Count == 0 && !animator.CheckAnimations("power"))
            animator.Power();

        
     }

    public override void PowerSound()
    {
        audioM.Play("Power");
    }

    public override void DashSound()
    {
        
    }

    

}

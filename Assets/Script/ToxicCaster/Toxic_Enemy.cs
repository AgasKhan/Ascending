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
       
    }

    public override void AuxiliarSound()
    {
     
    }

    public override void DeathSound()
    {
        audioM.Play("Death");
    }

    void MyAwake()
    {
        animator.functions.AddRange
        (
           new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               {"power", ActivePower},
               {"shoot", Shoot},
               { "offMesh", OffMesh}

           }
        );
    }

    
    void MyUpdate()
    {
        
        if (playerDetF && attackDelay.Chck)
        {
            attackDelay.Reset();

            animator.Attack();

            //Shoot();
        }
        else if (ObjectSpawnOnDamage.Count == 0 && !animator.CheckAnimations("power"))
            animator.Power();

        
     }

    public override void PowerSound()
    {
        audioM.Play("Power");
    }

    public override void DashSound()
    {
        
    }


    public override void OffMesh()
    {
        DeathSound();
    }
}

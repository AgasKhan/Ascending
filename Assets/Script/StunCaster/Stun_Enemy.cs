using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Enemy : Enemy_Character
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
               {"attackMelee", AttackMelee},
               { "endAttackMelee", EndAttackMelee},
               { "offMesh", OffMesh}

           }
        );

    }

    void MyUpdate()
    {
        
        if (playerDetF && attackDelay.Chck)
        {
            attackDelay.Reset();

            //animator.Power();

            animator.Attack();

            //ActivePower();
        }
    }

    public override void PowerSound()
    {
        audioM.Play("Attack");
    }

    public override void DashSound()
    {
      
    }

    public override void OffMesh()
    {
        DeathSound();
    }

}

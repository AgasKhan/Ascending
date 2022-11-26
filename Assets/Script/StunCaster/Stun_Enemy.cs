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
               {"attackMelee", AttackMelee},
               { "endAttackMelee", EndAttackMelee}

           }
        );

    }

    void MyUpdate()
    {
        
        if (playerDetF && attackDelay.Chck())
        {
            attackDelay.Reset();

            //animator.Power();

            animator.Attack();

            //ActivePower();
        }
    }

    public override void PowerSound()
    {
        throw new System.NotImplementedException();
    }

    public override void DashSound()
    {
        throw new System.NotImplementedException();
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

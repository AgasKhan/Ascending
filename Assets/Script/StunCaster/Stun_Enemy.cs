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

        MyStarts += () => { ReplaceFirstPower<Stun_Powers>(); };

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
}

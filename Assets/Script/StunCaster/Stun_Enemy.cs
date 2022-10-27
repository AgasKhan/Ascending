using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Enemy : Enemy_Character
{
    private void Awake()
    {

        animator.functions.AddRange(
           new Pictionary<string, AnimatorController.PrototypeFunc>[]
           {

                new Pictionary<string, AnimatorController.PrototypeFunc>("attackMelee", AttackMelee),
                new Pictionary<string, AnimatorController.PrototypeFunc>("endAttackMelee", EndAttackMelee)

           }

       );
    }

    protected override void MyUpdate()
    {
        
        if (playerDetF && attackDelay.Chck())
        {
            attackDelay.Reset();

            //animator.Power();

            animator.Attack();

            //ActivePower();
        }

        base.MyUpdate();
    }
}

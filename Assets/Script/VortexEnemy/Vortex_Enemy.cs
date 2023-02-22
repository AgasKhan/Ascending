using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Enemy : Enemy_Character
{
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
               {"power",ActivePowerDown},
               {"shoot",Shoot},
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
}

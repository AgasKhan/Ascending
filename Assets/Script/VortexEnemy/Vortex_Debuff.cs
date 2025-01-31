using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Debuff : Debuff_FatherPwDbff
{

    float originalSpeed;

    public override void OnEnterState(Character me)
    {
        originalSpeed = me.maxSpeed;

        me.maxSpeed *= 0.5f;
        me.movement.maxSpeed *= 0.5f;
    }

    public override void OnExitState(Character me)
    {
        me.maxSpeed = originalSpeed;
    }
}

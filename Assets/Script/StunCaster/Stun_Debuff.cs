using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Debuff : Debuff_FatherPwDbff
{
    [SerializeField] 
    float _stunDamage;

    public override void OnEnterState(Character me)
    {
        me.health.Substract(_stunDamage);
        me.movement.isDisable = true;
    }

    public override void OnExitState(Character me)
    {
        me.movement.isDisable = false;
    }
}

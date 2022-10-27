using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Powers : Powers_FatherPwDbff
{
    float _attackCooldown;

    public override void Activate(Character me)
    {
        me.AddPowerObjectSpawn(SchPowerObject("Vortex"));
    }

    public override void On(Character me)
    {
        _attackCooldown = me.damage;
        me.damage -= 1;
    }

    public override void Off(Character me)
    {
        me.damage = _attackCooldown;
    }
}

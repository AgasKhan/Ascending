using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Powers : Powers_FatherPwDbff
{

    float originalSpeed;
    public override void OnEnterState(Character me)
    {
        originalSpeed = me.maxSpeed;
        me.maxSpeed *= 1.5f;
        me.AddDebuffToAplicate<Vortex_Debuff>();
    }

    public override void OnExitState(Character me)
    {
        me.maxSpeed = originalSpeed;
        me.RemoveDebuffToAplicate<Vortex_Debuff>();
    }

    public override void ButtonEvent(Character me, float timePressed)
    {
        me.AddPowerObjectSpawn(SchPowerObject("Vortex"));
    }
}

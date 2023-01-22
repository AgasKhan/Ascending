using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Powers : Powers_FatherPwDbff
{

    float originalSpeed;

    public override void Activate()
    {
        me.AddPowerObjectSpawn(SchPowerObject("Vortex"));
    }

    public override void On()
    {
        originalSpeed = me.maxSpeed;
        me.maxSpeed *= 1.5f;
        me.AddDebuffToAplicate<Vortex_Debuff>();
    }

    public override void Off()
    {
        me.maxSpeed = originalSpeed;
        me.RemoveDebuffToAplicate<Vortex_Debuff>();
    }
}

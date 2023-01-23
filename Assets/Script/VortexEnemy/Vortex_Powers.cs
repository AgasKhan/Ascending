using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Powers : Powers_FatherPwDbff
{

    float originalSpeed;
    public override void On(Character me)
    {
        originalSpeed = me.maxSpeed;
        me.maxSpeed *= 1.5f;
        me.AddDebuffToAplicate<Vortex_Debuff>();

        stateButton.on = (number) => me.AddPowerObjectSpawn(SchPowerObject("Vortex"));
    }

    public override void Off(Character me)
    {
        me.maxSpeed = originalSpeed;
        me.RemoveDebuffToAplicate<Vortex_Debuff>();
    }
}

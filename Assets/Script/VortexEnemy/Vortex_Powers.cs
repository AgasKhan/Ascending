using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Powers : Powers_FatherPwDbff
{

    Pictionarys<Character, float> originalSpeed = new Pictionarys<Character, float>();

    public override void Activate(Character me)
    {
        me.AddPowerObjectSpawn(SchPowerObject("Vortex"));
    }

    public override void On(Character me)
    {

        if (!originalSpeed.ContainsKey(me))
        {
            originalSpeed.Add(me, me.movement.maxSpeed);
            me.movement.maxSpeed *= 1.5f;
        }
            
        me.AddDebuffToAplicate<Vortex_Debuff>();
        
    }

    public override void Off(Character me)
    {
        me.movement.maxSpeed = originalSpeed[me];
        originalSpeed.Remove(me);
        me.RemoveDebuffToAplicate<Vortex_Debuff>();
    }
}

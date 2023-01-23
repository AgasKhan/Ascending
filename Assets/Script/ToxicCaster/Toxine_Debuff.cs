using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxine_Debuff : Debuff_FatherPwDbff
{
    float _toxineDameg;

    Timer toDamage;
    public override void On(Character me)
    {
        on_Update = MyUpdate;
        _toxineDameg = 2;
        toDamage = TimersManager.Create(0.5f);

        particlesString = "ToxineParticle";
    }

    public override void Off(Character me)
    {
        TimersManager.Destroy(toDamage);
    }

    void MyUpdate(Character me)
    {
        if (toDamage.Chck)
        {
            me.health.Substract(_toxineDameg);
            toDamage.Reset();
        }
    }


}
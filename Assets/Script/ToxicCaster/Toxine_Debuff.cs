using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxine_Debuff : Debuff_FatherPwDbff
{
    float _toxineDameg;

    Timer toDamage;

    public override void On()
    {
        on_Update = MyUpdate;
        _toxineDameg = 2;
        toDamage = TimersManager.Create(0.5f);
    }

    public override void Off()
    {
       
    }

    void MyUpdate()
    {
        if (toDamage.Chck)
        {
            me.health.Substract(_toxineDameg);
            toDamage.Reset();
        }
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Debuff : Debuff_FatherPwDbff
{
    protected override void Aplicate(Character a)
    {
        on_Update = InTimePropio;

        DeAplicate = Off;

        print(a.name + " on Fire");

        a.health.Substract(1);
    }

    void Off(Character a)
    {
        print(a.name + " Fire extingued");
    }

    void InTimePropio(Character a)
    {
        if (a.MyCooldowns[dbffTimerName].Chck)
        {
            a.health.Substract(1);
            a.MyCooldowns[dbffTimerName].Reset();
        }
    }

    
}

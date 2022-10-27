using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Debuff : Debuff_FatherPwDbff
{

    protected override void Aplicate(Character a)
    {
        on_Update = MyUpdate;

        DeAplicate = MyDeaplicate;

    }

    void MyUpdate(Character a)
    {
        print(a.name + " esta en el vortice");

        if (a.MyCooldowns[dbffTimerName].Chck())
        {
            Remove(a);
        }
    }

    void MyDeaplicate(Character b)
    {
        
    }

}

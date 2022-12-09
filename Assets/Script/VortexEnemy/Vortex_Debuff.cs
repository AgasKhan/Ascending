using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex_Debuff : Debuff_FatherPwDbff
{

    Pictionarys<Character, float> originalSpeed = new Pictionarys<Character, float>();

    protected override void Aplicate(Character a)
    {
        on_Update = MyUpdate;

        DeAplicate = MyDeaplicate;

        if (!originalSpeed.ContainsKey(a))
            originalSpeed.Add(a, a.maxSpeed);

        a.maxSpeed *= 0.5f;
    }

    void MyUpdate(Character a)
    {
        print(a.name + " esta relentizado");

        if (a.MyCooldowns[dbffTimerName].Chck)
        {
            Remove(a);
        }
    }

    void MyDeaplicate(Character a)
    {
        a.maxSpeed = originalSpeed[a];
        originalSpeed.Remove(a);
    }

}

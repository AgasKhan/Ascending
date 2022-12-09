using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxine_Debuff : Debuff_FatherPwDbff
{

    [SerializeField] 
    float _toxineDameg;

    string _cdString = "toxineDmg";

    protected override void Aplicate(Character a)
    {
        on_Update = MyUpdate;

        AddCooldown(_cdString, 1 , a);
    }

    void MyUpdate(Character a)
    {
        print(a.name + " tiene toxina");

        if (a.MyCooldowns[_cdString].Chck)
        {
            a.health.Substract(_toxineDameg);
            a.MyCooldowns[_cdString].Reset();
        }

        if (a.MyCooldowns[dbffTimerName].Chck)
        {
            Remove(a);
        }
    }
}
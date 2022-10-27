using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Debuff : Debuff_FatherPwDbff
{
    [SerializeField] 
    float _stunDamage;

    protected override void Aplicate(Character a)
    {
        on_Update = MyUpdate;

        DeAplicate = MyDeaplicate;

        a.health.Substract(_stunDamage);

        print(a.name + " ha sido stuneado");
        a.movement.isDisable = true;
    }


    void MyUpdate(Character a)
    {
        if (a.MyCooldowns[dbffTimerName].Chck())
        {
            Remove(a);
        }
    }

    void MyDeaplicate (Character b)
    {
        b.movement.isDisable = false;
        print("El enemigo ha dejado de ser stuneado");
    }


}

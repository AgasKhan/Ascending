using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun_Powers : Powers_FatherPwDbff
{
    [SerializeField]
    Vector2Int IceGenerator;
    public override void On(Character me)
    {
        IceGenerator = SchPowerObject("IceGenerator");

        me.AddDebuffToAplicate<Stun_Debuff>();
    }

    public override void Off(Character me)
    {
        base.Off(me);

        me.RemoveDebuffToAplicate<Stun_Debuff>();
    }

    public override void Activate(Character me)
    {
        me.AddPowerObjectSpawn(IceGenerator);
    }


}

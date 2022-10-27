using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxine_Powers : Powers_FatherPwDbff
{
    float _originalJumpStrength;

    public override void Activate(Character me)
    {
        me.AddPowerObjectSpawn(SchPowerObject("toxicSmoke"));
    }

    public override void On(Character me)
    {
        _originalJumpStrength = me.jumpStrength;
        me.jumpStrength *= 1.5f;
        me.AddDebuffToAplicate<Toxine_Debuff>();
    }

    public override void Off(Character me)
    {
        base.Off(me);
        me.jumpStrength = _originalJumpStrength;
        me.RemoveDebuffToAplicate<Toxine_Debuff>();
    }

    /*
    private void Update()
    {
        //el elemento que me agrega la toxina al colisionar
        me.AddPowerObjectSpawn(SchPowerObject("toxicDebuffCaster"));
    }
    */

}

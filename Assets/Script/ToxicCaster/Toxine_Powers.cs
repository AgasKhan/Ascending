using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toxine_Powers : Powers_FatherPwDbff
{
    float _originalJumpStrength;
    public override void OnEnterState(Character me)
    {
        _originalJumpStrength = me.jumpStrength;
        me.jumpStrength *= 1.5f;
        me.AddDebuffToAplicate<Toxine_Debuff>();
    }

    public override void OnExitState(Character me)
    {
        me.jumpStrength = _originalJumpStrength;
        me.RemoveDebuffToAplicate<Toxine_Debuff>();
    }

    public override void ButtonEvent(Character me,float timePressed)
    {
        me.AddPowerObjectSpawn(SchPowerObject("toxicSmoke"));
    }
}

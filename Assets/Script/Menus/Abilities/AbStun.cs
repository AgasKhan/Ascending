using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbStun : AbilitiesParent
{
    public override void ActiveAbility()
    {
        base.ActiveAbility();
        SkillTreeManager.SwitchPowers();
    }

    public override void ActionOnStart()
    {

    }

    public override Abilities.Ability Create()
    {
        throw new NotImplementedException();
    }

    public override Type ReturnType()
    {
        throw new NotImplementedException();
    }
}

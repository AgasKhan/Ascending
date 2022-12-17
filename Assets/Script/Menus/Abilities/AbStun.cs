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
}

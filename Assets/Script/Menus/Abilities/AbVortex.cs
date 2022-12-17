using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbVortex : AbilitiesParent
{
    public override void ActiveAbility()
    {
        base.ActiveAbility();
        SkillTreeManager.SwitchPowers();
    }

    private void Start()
    {
        VinculatedAbilities<Abilities.PowerInit<Vortex_Powers>>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbToxic : AbilitiesParent
{
    public override void ActiveAbility()
    {
        base.ActiveAbility();
        SkillTreeManager.SwitchPowers();
    }

    protected override void Config()
    {
        base.Config();
        MyAwakes += MyAwake;
    }
    private void MyAwake()
    {
        VinculatedAbilities<Abilities.PowerInit<Toxine_Powers>>();
    }
}

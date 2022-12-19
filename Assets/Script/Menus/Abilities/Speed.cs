using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : AbilitiesParent
{
    protected override void Config()
    {
        base.Config();
        MyAwakes += MyAwake;
    }
    private void MyAwake()
    {
        VinculatedAbilities<Abilities.Speed>();
    }
}

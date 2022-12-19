using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToArrive : AbilitiesParent
{
    protected override void Config()
    {
        base.Config();
        MyAwakes += MyAwake;
    }
    private void MyAwake()
    {
        VinculatedAbilities<Abilities.TimeToArrive>();
    }
}

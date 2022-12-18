using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : AbilitiesParent
{
    private void Start()
    {
        VinculatedAbilities<Abilities.ChargeDagger>();
    }

    public override void UnlockNextButton()
    {
        base.UnlockNextButton();
    }

}

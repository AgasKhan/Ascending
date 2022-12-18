using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialDaggers : AbilitiesParent
{
    public int count;
    private void Start()
    {
        VinculatedAbilities<Abilities.InitialDaggers>();

        ((Abilities.InitialDaggers)myAbility).count = count;
    }
}

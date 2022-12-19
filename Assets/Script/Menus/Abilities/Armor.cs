using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : AbilitiesParent
{
    private void Start()
    {
        VinculatedAbilities<Abilities.Armor>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : AbilitiesParent
{

    private void Start()
    {
        VinculatedAbilities<Abilities.HealthPoints>();
    }

}

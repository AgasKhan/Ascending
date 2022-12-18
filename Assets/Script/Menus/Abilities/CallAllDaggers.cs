using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAllDaggers : AbilitiesParent
{
    private void Start()
    {
        VinculatedAbilities<Abilities.CallAllDaggers>();
    }

}

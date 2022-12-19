using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialDaggers : AbilitiesParent
{
    [SerializeField]
    public int count;

    Abilities.InitialDaggers reference;

    protected override void Config()
    {
        base.Config();
        MyAwakes += MyAwake;
    }
    private void MyAwake()
    {
        reference = VinculatedAbilities<Abilities.InitialDaggers>();
    }

    public override void ActiveAbility()
    {
        myAbility.active = true;
        reference.count.Add(name, count);
    }

    public override void DeactiveAbility()
    {
        reference.count.Remove(name);

        if(reference.count.count==0)
            myAbility.active = false;
    }
}

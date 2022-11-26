using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoFueguito_Enemy : Enemy_Character
{
    public override void AttackSound()
    {
        throw new System.NotImplementedException();
    }

    public override void AuxiliarSound()
    {
        throw new System.NotImplementedException();
    }

    public override void DashSound()
    {
        throw new System.NotImplementedException();
    }

    public override void DeathSound()
    {
        throw new System.NotImplementedException();
    }

    public override void PowerSound()
    {
        throw new System.NotImplementedException();
    }

    public override void WoRSoundLeft()
    {
        throw new System.NotImplementedException();
    }

    public override void WoRSoundRight()
    {
        throw new System.NotImplementedException();
    }

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
       
        MyUpdates += MyUpdate;

    }

    void MyAwake()
    {
        
    }

     void MyUpdate()
    {

    }

    
}

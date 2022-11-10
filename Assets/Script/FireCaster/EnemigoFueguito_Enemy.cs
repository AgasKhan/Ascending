using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoFueguito_Enemy : Enemy_Character
{

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

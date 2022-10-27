using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : EffectArea
{

    public float _myScale;
    public int force;

    protected override void MyAwake()
    {
        base.MyAwake();
        _myScale = transform.lossyScale.x;
    }

    private void OnTriggerStay (Collider other)
    {

        ChckAddAffected(other.gameObject);


    }


    protected override void MyFixedUpdate()
    {
        base.MyFixedUpdate();

        for (int i = affected.Count - 1; i >= 0; i--)
        {
            if ((affected[i].go.transform.position - transform.position).sqrMagnitude < (_myScale * _myScale))
                affected[i].absordedRB.AddExplosionForce(force * -1, transform.position, _myScale);
            else
                affected.RemoveAt(i);
        }
    }


}

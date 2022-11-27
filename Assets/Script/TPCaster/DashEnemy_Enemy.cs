using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy_Enemy : Enemy_Character
{
    [SerializeField] float _angleOfDashAim=15;

    protected override void Config()
    {
        base.Config();

        //MyAwakes += MyAwake;
        
        MyUpdates += MyUpdate;

    }

    public override void Dash()
    {
        if (Utilitys.DeltaAngleY(scopedPoint - transform.position, out float angle, transform.rotation) < _angleOfDashAim)
        {
            animator.transform.forward = scopedPoint - transform.position;
        }
        base.Dash();
    }

    public override void AttackSound()
    {
        throw new System.NotImplementedException();
    }

    public override void AuxiliarSound()
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

    public override void DashSound()
    {
        audioM.Play("EnemyDash");
    }

    public override void WoRSoundLeft()
    {
        throw new System.NotImplementedException();
    }

    public override void WoRSoundRight()
    {
        throw new System.NotImplementedException();
    }
/*
    void MyAwake()
    {
        
        animator.functions.AddRange
        (
           new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               //{"dash", Dash}
           }
        );
    }
*/
    void MyUpdate()
    {

        //Physics.Raycast(new Ray(transform.position, transform.forward),  out RaycastHit rayCastHit, layerMask);

        if (attackDelay.Chck() && Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit rayCastHit, layerMask) && rayCastHit.distance <= 5 && rayCastHit.collider.gameObject.CompareOneTags("Player", "Traspasable"))
        {
            scopedPoint = rayCastHit.point;

            attackDelay.Reset();

            animator.Dash(true);
        }
    }

  
}

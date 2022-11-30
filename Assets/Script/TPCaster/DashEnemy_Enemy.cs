using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy_Enemy : Enemy_Character
{
    [SerializeField] float _angleOfDashAim=15;

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
        
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

    }

    public override void AuxiliarSound()
    {

    }

    public override void DeathSound()
    {
        audioM.Play("Death");
    }

    public override void PowerSound()
    {

    }

    public override void DashSound()
    {
        audioM.Play("EnemyDash");
    }
    public override void OffMesh()
    {
        DeathSound();
    }

    void MyAwake()
    {
        animator.functions.AddRange
        (
           new Pictionarys<string, AnimatorController.PrototypeFunc>
           {
               { "offMesh", OffMesh}
           }
        );
    }

    void MyUpdate()
    {

        //Physics.Raycast(new Ray(transform.position, transform.forward),  out RaycastHit rayCastHit, layerMask);

        if ((attackDelay.Chck() && playerDetF || Raycast(new Ray(transform.position, transform.forward)) && scoped.CompareTag("Traspasable")) && (scopedPoint - transform.position).sqrMagnitude < 25)
        {
            attackDelay.Reset();

            animator.Dash(true);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy_Enemy : Enemy_Character
{
    [SerializeField] float _angleOfDashAim=15;

    System.Action OtherUpdates;

    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
        OtherUpdates = MyUpdates;
        MyUpdates = MyUpdate;

    }

    public override void Dash()
    {
        Vector3 aux = scopedPoint - transform.position;

        if (Utilitys.DeltaAngleY(aux, out float angle, transform.rotation) < _angleOfDashAim)
        {
            aux.y = 0;

            animator.transform.forward = aux;
        }

        base.Dash();

    }
    public override void DashSound()
    {
        audioM.Play("EnemyDash");
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

        if ((attackDelay.Chck && (playerDetF || (Raycast(new Ray(transform.position, transform.forward)) && scoped.CompareTag("Traspasable")))) && (scopedPoint - transform.position).sqrMagnitude < 25)
        {
            attackDelay.Reset();

            animator.Dash(true);
        }

        if (!movement.dash)
            OtherUpdates();

        if((scopedPoint - transform.position).sqrMagnitude < 4)
            transform.forward = scoped.transform.position - transform.position;
    }
}

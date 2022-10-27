using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_KnifeElements : KnifeElements
{

    Knifes knife;

    public void Attack()
    {
        if (knife == null)
            return;

        knife.reference.parent = null;

        knife.movement.kinematic = false;

        knife.daggerScript.active = true;

        //print(character.cameraScript.hitPoint - knife.reference.position);

        //knife.movement.Move(character.scoped.point-knife.reference.position, knife.movement.maxSpeed * 10);

        knife.movement.Dash(character.scopedPoint - knife.reference.position);

        elements.Remove(knife);

        knife = null;

        PreAttack();
    }

    public void PreAttack()
    {
        if(elements.Count>0)
        {
            knife = elements[0];
            knife.reference.parent = transform;
        }
    }

    public void CancelAttack()
    {
        if (knife == null)
            return;

        knife.reference.parent = Other.transform;
        knife = null;

    }

    // Update is called once per frame
    void Update()
    {
        if(knife!=null)
        {
            knife.reference.localPosition = Vector3.Slerp(transform.GetChild(0).localPosition, distance, Time.deltaTime);

           
            Vector3 rot = character.scopedPoint - knife.reference.position;

            knife.movement.RotateToDir(rot, new Vector3(90,0,0));

            //knife.reference.LookAt(character.cameraScript.hitPoint);
        }

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_KnifeElements : KnifeElements
{

    public bool UnlockHitScan = false;

    public float maxPressedTime = 3;

    public float relationXtime = 1;

    public float chargePercentage;

    [SerializeReference]
    Knifes knife;

    float time=1;

    public float ChargeAttack()
    {
        time += Time.deltaTime;

        chargePercentage =  (time - 1) / maxPressedTime;

        if (chargePercentage > 1)
            chargePercentage = 1;

        return chargePercentage;
    }

    public void Attack()
    {
        if (knife == null)
            return;

        float atackMultiply=1;

        elements.Remove(knife);

        knife.reference.parent = null;

        knife.movement.kinematic = false;

        knife.daggerScript.active = true;

        //print(character.cameraScript.hitPoint - knife.reference.position);

        //knife.movement.Move(character.scoped.point-knife.reference.position, knife.movement.maxSpeed * 10);

        if(time-1 < maxPressedTime)
            atackMultiply = 1 + relationXtime * time / 10;
        else
        {
            if(UnlockHitScan)
            {
                knife.daggerScript.SetLine(character.scopedPoint,transform.position);
                knife.movement.transform.position = (character.scopedPoint - (character.scopedPoint - knife.reference.position).normalized);
            }
            else
                atackMultiply = 1 + maxPressedTime * relationXtime / 10;
        }


        knife.movement.Dash(character.scopedPoint - knife.reference.position, atackMultiply);

        time = 1;

        knife = null;

        chargePercentage = 0;

        PreAttack();
    }

    public void PreAttack()
    {
        if (elements.Count > 0)
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

            knife.reference.localPosition = Vector3.Lerp(knife.reference.localPosition, distance + Vector3.forward*2*(1-chargePercentage), Time.deltaTime);

            Vector3 rot = character.scopedPoint - knife.reference.position;

            knife.movement.RotateToDir(rot, new Vector3(90,0,0));

            //knife.reference.LookAt(character.cameraScript.hitPoint);
        }

        
    }
}

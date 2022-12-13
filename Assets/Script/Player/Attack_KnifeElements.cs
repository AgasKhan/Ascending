using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Attack_KnifeElements : KnifeElements
{

    public bool UnlockHitScan = false;

    public float maxPressedTime = 3;

    public float relationXtime = 1;

    public float chargePercentage;

    public LensDistortion lens;

    float zoomDistorsion;

    [SerializeReference]
    Knifes knife;

    PostProcessVolume volume;

    Tim charge;

    Tim zoomTozero;

    public float ChargeAttack()
    {
        chargePercentage = charge.Substract(Time.deltaTime);

        if (chargePercentage < 1 && chargePercentage!=0)
        {
            zoomDistorsion = chargePercentage;    
        }
        else if (chargePercentage >= 1)
        {
            chargePercentage = 1;
            zoomDistorsion = 1 + 0.05f*Mathf.Sin(Time.time*15);
        }


        zoomTozero.Reset();

        return chargePercentage;
    }

    public bool Attack()
    {
        if (knife == null)
            return false;

        float atackMultiply=1;

        Vector3 dir = (character.scopedPoint - knife.reference.position).normalized;

        Damage dmg = new Damage();

        dmg.amount = character.damage;

        dmg.debuffList = character.debuffToAplicate;

        dmg.velocity = dir*200;

        elements.Remove(knife);

        knife.reference.parent = null;

        knife.movement.kinematic = false;

        knife.movement.useGravity = false;

        knife.daggerScript.active = true;



        //print(character.cameraScript.hitPoint - knife.reference.position);

        //knife.movement.Move(character.scoped.point-knife.reference.position, knife.movement.maxSpeed * 10);

        if(charge.Percentage() < 1)
        {
            atackMultiply = 1 + relationXtime * charge.Percentage() / 10;
            dmg.velocity *= atackMultiply;
        }
        else
        {
            if(!UnlockHitScan)
            {
                atackMultiply = 1 + maxPressedTime * relationXtime / 10;
                dmg.velocity *= atackMultiply;
            }
            else
            {
                knife.daggerScript.SetLine(character.scopedPoint, knife.reference.position);
                knife.movement.transform.position = character.scopedPoint - dir;
                dmg.velocity *= 1 + maxPressedTime * relationXtime;

                Utilitys.LerpInTime(1f, 0.3f, 0.25f, Mathf.Lerp, (save) => { Time.timeScale = save; });

                TimersManager.Create(0.25f, 
                    ()=>
                    {
                        Utilitys.LerpInTime(0.3f, 1f, 0.75f, Mathf.Lerp, (save) => { Time.timeScale = save; });
                    }, true, true);
            }
        }

        if (dmg.velocity.sqrMagnitude > 500 * 500)
            dmg.velocity = dmg.velocity.normalized * 500;

        knife.daggerScript.Throw(dmg, dir, atackMultiply);

        knife = null;

        charge.Reset();

        chargePercentage = 0;

        zoomDistorsion = -1;
        zoomTozero.Reset();

        PreAttack();

        RefreshUI();

        return true;
    }

    public void PreAttack()
    {
        if (elements.Count > 0)
        {
            knife = elements[0];
            knife.reference.parent = transform;
            knife.daggerScript.pause = true;
        }
    }

    public void CancelAttack()
    {
        if (knife == null)
            return;

        knife.reference.parent = Other.transform;
        knife = null;
        chargePercentage = 0;
        charge.Reset();
    }

    // Update is called once per frame

    private void Awake()
    {
        lens = ScriptableObject.CreateInstance<LensDistortion>();
        lens.enabled.Override(true);
        lens.intensity.Override(0);
        volume = PostProcessManager.instance.QuickVolume(12, -1, lens);

        zoomTozero = new Tim(0.5f);
        charge = new Tim(maxPressedTime);
 
    }

    private void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(volume, true, true);
    }

    void Update()
    {

        if(knife!=null)
        { 

            knife.reference.localPosition = Vector3.Slerp(knife.reference.localPosition, distance + Vector3.forward*(1-chargePercentage), Time.deltaTime);

            //lens.intensity.Override(50* chargePercentage);

            Vector3 rot = character.scopedPoint - knife.reference.position;

            knife.movement.RotateToDir(rot, new Vector3(90,0,0));

            //Debug.DrawRay(knife.reference.position, character.scopedPoint - knife.reference.position, Color.white);
            Ray ray = new Ray(knife.reference.position, character.scopedPoint - knife.reference.position);
            RaycastHit raycastHit;

            if(Physics.Raycast(ray , out raycastHit, 10, character.cameraScript.layerMask) && !raycastHit.collider.CompareTag("Player") && (Mathf.Abs(raycastHit.point.sqrMagnitude - character.scopedPoint.sqrMagnitude)> (0.05f*0.05f *raycastHit.distance)))
            {
                //knife.daggerScript.SetLine(knife.reference.position, raycastHit.point);
                MainHud.PunteroPos(Camera.main.WorldToScreenPoint(raycastHit.point));
            }
            //knife.reference.LookAt(character.cameraScript.hitPoint);
        }
        else if(transform.childCount!=0)
        {
            transform.GetChild(0).parent = Other.transform;
        }


        if (Mathf.Abs(Mathf.Abs(lens.intensity.value) - Mathf.Abs(zoomDistorsion * 50)) < 1f)
            zoomDistorsion = Mathf.Lerp(zoomDistorsion, 0, zoomTozero.Substract(Time.deltaTime));
            
        lens.intensity.Override(Mathf.Lerp(lens.intensity.value, 50 * zoomDistorsion, Time.deltaTime*15));

    }
}

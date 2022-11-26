using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSmoke : EffectArea
{
    AudioManager audioM;
    /*
    private void Awake()
    {
        audioM = GetComponent<AudioManager>();
        ImpactToxineAudio();
    }
    */
    private void OnTriggerStay(Collider other)
    {
       
        bool check = true;

       
        for (int i = 0; i < affected.Count; i++)
        {
            if(affected[i].go == other.gameObject)
            {
                check = false;

                affected[i].ChckandSubsHealth(dmg);
                return;
            }       
        }

        if (check && other.gameObject.CompareOneTags(Tag.life))
        {
            AddAffected(other.gameObject, coolDown);
            affected[affected.Count - 1].ChckandSubsHealth(dmg);
        }         
    }
    
    void ImpactToxineAudio()
    {
        audioM.Play("ImpactToxine");
    }

}
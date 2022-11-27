using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Teleport : MonoBehaviour
{

   
    private void OnEnable()
    {
        Player_Character player = GameManager.player;




        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (var item in colliders)
        {
            if(!item.gameObject.CompareTag("Dagger") && item.gameObject.CompareTags(Tag.rb))
            {

                LensDistortion lens = ScriptableObject.CreateInstance<LensDistortion>();
                lens.enabled.Override(true);
                lens.intensity.Override(0);
                lens.centerX.Override(0);
                lens.centerY.Override(0);

                PostProcessVolume volume = PostProcessManager.instance.QuickVolume(12, 100f, lens);

                Timers.Create(0.5f, ()=> {

                    DebugPrint.Log("comenzo la animacion del teleport");

                }, ()=> {

                    Vector3 vector = Camera.main.WorldToScreenPoint(item.transform.position);

                    lens.intensity.Override(Mathf.Lerp(lens.intensity.value, +75, Time.deltaTime*5));
                    lens.scale.Override(Mathf.Lerp(lens.scale.value, 0.01f, Time.deltaTime*5));

                },()=> {
                    
                    Timers.Create(0.5f, () => {

                        DebugPrint.Log("comenzo la finalizacion de la animacion de teleport");
                        item.transform.position = player.transform.position;
                        player.transform.position = transform.position;

                    }, () => {
                        Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);

                        lens.intensity.Override(Mathf.Lerp(lens.intensity.value, player.atackElements.lens.intensity.value, Time.deltaTime*15));
                        lens.scale.Override(Mathf.Lerp(lens.scale.value, 1, Time.deltaTime*15));

                    }, () => {

                        RuntimeUtilities.DestroyVolume(volume, true, true);

                    });

                });

               
                
                break;
            }

        }

        gameObject.SetActive(false);
    }


    
}

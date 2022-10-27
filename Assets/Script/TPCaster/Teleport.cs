using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

   
    private void OnEnable()
    {
        GameObject player = GameManager.player.gameObject;


        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (var item in colliders)
        {
            if(!item.gameObject.CompareTag("Dagger") && item.gameObject.CompareTags(Tag.rb))
            {
                item.transform.position = player.transform.position;

                player.transform.position = transform.position;
                
                break;
            }

        }

        gameObject.SetActive(false);
    }


    
}

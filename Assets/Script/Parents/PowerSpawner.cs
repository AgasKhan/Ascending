using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerSpawner : MonoBehaviour
{
    private void OnEnable()
    {


        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (var item in colliders)
        {
            if (!item.gameObject.CompareTag("Dagger") && item.gameObject.CompareTags(Tag.rb))
            {
                
                Detect(item.GetComponent<MoveRb>());

                break;
            }

        }

        gameObject.SetActive(false);
    }

    protected abstract void Detect(MoveRb item);
}

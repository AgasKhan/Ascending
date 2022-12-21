using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerSpawner : MonoBehaviour
{
    public float radiusChck = 0.1f;

    private void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusChck);

        foreach (var item in colliders)
        {
            if (!item.gameObject.CompareTag("Dagger") && item.gameObject.TryGetComponent(out MoveRb moveRb))
            {
                Detect(moveRb);

                break;
            }
        }

        gameObject.SetActive(false);
    }

    protected abstract void Detect(MoveRb item);
}

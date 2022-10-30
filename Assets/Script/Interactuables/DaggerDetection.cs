using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerDetection : MonoBehaviour
{
    [SerializeField]
    LogicActive[] active;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Dagger"))
            return;

        foreach (var item in active)
        {
            item.Activate();
        }
    }
}

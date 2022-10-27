using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpObject : MonoBehaviour
{
    public GameObject Destination;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Character>())
            other.transform.position = Destination.transform.position;
    }
}

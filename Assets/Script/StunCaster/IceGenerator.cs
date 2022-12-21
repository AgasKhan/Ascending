using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2Int Ice;

    private void Awake()
    {
        Ice = PoolObjects.SrchInCategory("Stun", "Ice");
    }

   

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactuable_LogicActive : LogicActive
{
    public Collider col;
    public float pressedTime;
    public string DisplayText;
    public bool diseable;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactuable_LogicActive : LogicActive
{
    public Collider col;
    public float pressedTime;
    public string DisplayText;
    public bool diseable;
    public float distanceDetection;

    Player_Character player;

    private void Start()
    {
        col = GetComponent<Collider>();
        player = GameManager.player;
    }

    private void FixedUpdate()
    {
     
        if(!diseable)
            col.enabled= (player.transform.position - transform.position).sqrMagnitude < (distanceDetection * distanceDetection);

    }

}

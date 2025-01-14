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

    protected Animator[] animator;
    
    Player_Character player;
    

    public void RefreshUi(Vector3 pos, float perc)
    {
        InteractiveObj.instance.LoadInfo(Controllers.active.ToString(), pos, perc);
        InteractiveObj.instance.subTittle.text=DisplayText;
    }

    private void Start()
    {
        col = GetComponent<Collider>();
        player = GameManager.player;

        animator = GetComponentsInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if(!diseable)
        {
            col.enabled = (player.transform.position - transform.position).sqrMagnitude < (distanceDetection * distanceDetection);
        }
    }
}

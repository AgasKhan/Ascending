using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrToSpecificPatrol : MoveTrToPatrol, ISetInt
{
    public int index
    {
        get
        {
            return patrol.iPatrulla;
        }
        set
        {
            patrol.iPatrulla = value;
            enabled = true;
        }
    }

    public int GetInt()
    {
        return index;
    }

    public void SetInt(int i)
    {
        index = i;
    }

    protected override void Config()
    {
        base.Config();

        MyAwakes+= () =>patrol.fsmPatrol.OnStartMove += OnStartMove;
    }

    void OnStartMove()
    {
        enabled = false;
    }

    
}

public interface ISetInt
{
    void SetInt(int i);

    int GetInt();

}
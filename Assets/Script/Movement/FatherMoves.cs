using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FatherMoves : MyScripts
{

    /// <summary>
    /// Si realiza los calculos que se le pasan
    /// </summary>
    public bool isDisable = false;

    /// <summary>
    /// La maxima velocidad para moverse (capea en este punto usando el move por defecto)
    /// </summary>
    public float maxSpeed;

    /// <summary>
    /// Con cuanta brusquedad acelera y desacelera
    /// </summary>
    public float desAceleration;


    abstract public void Move(Vector3 direction, float magnitude);

    public void Move(Vector3 direction)
    {
        Move(direction, maxSpeed);
    }

    public void MoveLocal(Vector3 direction)
    {
        MoveLocal(direction, maxSpeed);
    }

    public void MoveLocal(Vector3 direction, float magnitude)
    {
        if (direction.sqrMagnitude == 0)
            return;

        Vector3 localDir = transform.forward * direction.z + transform.up * direction.y + transform.right * direction.x;

        Move(localDir, magnitude);
    }



}

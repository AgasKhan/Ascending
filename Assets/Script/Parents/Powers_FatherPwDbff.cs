using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase padre de los poderes
/// </summary>
[System.Serializable]
abstract public class Powers_FatherPwDbff : FatherPwDbff
{
    /// <summary>
    /// Funcion que sera llamada cuando se lance la habilidad
    /// </summary>
    /// <param name="me">character duenio de la habilidad</param>
    //public State<float> stateButton = new State<float>();

    public abstract void ButtonEvent(Character me,float timePressed);

    /*
    public override void OnEnterState(Character me)
    {
        Controllers.power.eventDown += (number) => ButtonEvent(me, number);
    }
    */
}




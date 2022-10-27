using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveTr : FatherMoves
{

    /// <summary>
    /// desactiva la desaceleracion automatica
    /// </summary>
    protected bool desactivateDesaceleration=false;

    /// <summary>
    /// variable que almacena la realacion de la velocidad con respecto a su velocidad maxima
    /// </summary>
    [SerializeField]
    protected float relation;

    /// <summary>
    /// vector al que se mueve el objeto
    /// </summary>
    [SerializeField]
    Vector3 _velocity;

    /// <summary>
    /// vector de velocidad de entrada
    /// </summary>
    [SerializeField]
    Vector3 _velocityInput;

    public override void Move(Vector3 direction, float magnitude)
    {
        if(!isDisable)
            _velocityInput = (direction.normalized * magnitude);
    }

    protected override void MyAwake()
    {
        GameManager.AddTimeController(transform);
        _velocityInput = Vector3.zero;
    }

    /// <summary>
    /// Funcion que ejecuta el movimiento en si
    /// por defecto se encuentra dentro del update
    /// </summary>
    protected virtual void MoveOn()
    {
        if (desactivateDesaceleration && _velocity.sqrMagnitude > _velocityInput.sqrMagnitude)
            _velocity = _velocityInput;
        else
        {
            _velocity = Vector3.Lerp(_velocity, _velocityInput, Time.deltaTime * desAceleration);
        }

        transform.position = Vector3.MoveTowards(transform.position, _velocity + transform.position, maxSpeed * Time.deltaTime);
    }

    /// <summary>
    /// se mueve tomando en cuenta la distancia para de esta forma poder acelerar y frenar de forma realista
    /// </summary>
    /// <param name="dist"></param>
    protected void MoveTo(Vector3 dist)
    {
        relation = dist.sqrMagnitude / (Mathf.Pow((maxSpeed) / desAceleration, 2));

        if (relation > 1)
            relation = 1;

        Move(dist, maxSpeed * (relation));
    }

    protected override void MyUpdate()
    {
        MoveOn();
    }

    protected override void MyFixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    protected override void MyStart()
    {
        throw new System.NotImplementedException();
    }
}

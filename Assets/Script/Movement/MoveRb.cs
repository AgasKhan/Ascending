using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveRb : FatherMoves, IOnProyectileEnter
{

    /// <summary>
    /// string con el nombre del layer que pasara mientras ejecuta el dash
    /// </summary>
    public string layerDash;

    /// <summary>
    /// retorna si esta el kinematic activo o no
    /// </summary>
    public bool kinematic
    {
        get
        {
            return _myRigid.isKinematic;
        }

        set
        {
            _myRigid.isKinematic = value;
        }
    }

    public bool useGravity
    {
        get
        {
            return _myRigid.useGravity;
        }

        set
        {
            _myRigid.useGravity = value;
        }
    }

    /// <summary>
    /// activa o desactiva el drag general (las funciones permiten desactivar el calculo del limite de velocidad aparte)
    /// </summary>
    public bool eneableDrag = true;

    /// <summary>
    /// Vector de velocidad
    /// </summary>
    public Vector3 velocity3D
    {
        get;
        private set;
    }

    /// <summary>
    /// Vector de velocidad sin tomar en cuenta la gravedad
    /// </summary>
    public Vector3 velocity2D
    {
        get;
        private set;
    }

    /// <summary>
    /// si el dash esta activo
    /// </summary>
    public bool dash
    {
        get;
        private set;
    }

    /// <summary>
    /// realacion lineal con respecto a la velocidad maxima
    /// </summary>
    public float relation
    {
        get;
        private set;
    }


    /// <summary>
    /// Velocidad de impulso que se aplica al ejecutar el dash
    /// </summary>
    [SerializeField] 
    float _dashImpulse;

    /// <summary>
    /// Factor de rozamiento, el cual incrementa con la velocidad
    /// </summary>
    [SerializeField] 
    float _dragFactor;

    /// <summary>
    /// Referencia privada al rigibody
    /// </summary>
    Rigidbody _myRigid;

    /// <summary>
    /// vector de maxima velocidad con la misma direccion que el vector actual de velocidad
    /// </summary>
    Vector3 _maxVelocity2D;//

    /// <summary>
    /// si el objeto se esta moviendo o no
    /// </summary>
    bool _move = false;//

    /// <summary>
    /// se gurda la layer por defecto
    /// </summary>
    int _normalLayer;


    #region Movimiento Global

    public void ProyectileEnter(Damage damage)
    {        
        Move(damage.velocity, damage.velocity.magnitude, false);
    }


    public void Move(Vector3 direction, bool drag)
    { 
        Move(direction, maxSpeed, drag);
    }

    public void Move(Vector3 direction, float magnitude, bool drag)
    {
        bool auxDrag = eneableDrag;

        eneableDrag = drag;

        Move(direction, magnitude);

        eneableDrag = auxDrag;
    }

    override public void Move(Vector3 direction, float magnitude)
    {

        float aux = direction.sqrMagnitude;

        if (!isDisable && !dash && aux != 0)
        {
            Vector3 normalized;

            if (aux <= 1)
                normalized = direction;
            else
                normalized = direction.normalized;

            if (eneableDrag)
            {
                //_myRigid.velocity += _dragFactor * Time.fixedDeltaTime * desAceleration * ( (velocity2D * -( (aux >= 1) ? 1 : 1/( Mathf.Sqrt(aux) ) ) ) ) ; //Limita a la velocidad maxima
                _myRigid.velocity += _dragFactor * Time.fixedDeltaTime * desAceleration * ((velocity2D * -1)); //Limita a la velocidad maxima

            }

            _myRigid.velocity += magnitude * desAceleration * Time.fixedDeltaTime * normalized;
            
            _move = true;

        }
    }

    #endregion

    #region Movimiento Local

    public void MoveLocal(Vector3 direction, bool drag)
    {
        MoveLocal(direction, maxSpeed, drag);
    }

    public void MoveLocal(Vector3 direction, float magnitude, bool drag)
    {
        bool auxDrag = eneableDrag;

        eneableDrag = drag;

        MoveLocal(direction, magnitude);

        eneableDrag = auxDrag;

    }

    #endregion

    #region Movimiento especifico
    /// <summary>
    /// Aplica un impulso y cambia la capa de colision
    /// </summary>
    /// <param name="dir">vector direccion</param>
    public float Dash(Vector3 dir, float multiply =1)
    {
        var aux = _dashImpulse * multiply;

        Move(dir, aux);

        if(layerDash!=null && layerDash != "")
            gameObject.layer = LayerMask.NameToLayer(layerDash);
        
        dash = true;

        return aux;
    }


    /// <summary>
    /// Aplica un impulso y cambia la capa de colision
    /// </summary>
    /// <param name="dir">vector direccion</param>
    public void DashLocal(Vector3 dir, float multiply = 1)
    {
        MoveLocal(dir, _dashImpulse*multiply);

        if (layerDash != null && layerDash != "")
            gameObject.layer = LayerMask.NameToLayer(layerDash);

        dash = true;
    }

    public void CancelGravity()
    {
        _myRigid.velocity = new Vector3(_myRigid.velocity.x, 0, _myRigid.velocity.z);
    }

    public void Stop()
    {
        _myRigid.velocity = Vector3.zero;
    }

    #endregion


    void MyAwake()
    {
        _myRigid = GetComponent<Rigidbody>();

        _normalLayer = gameObject.layer;

        _maxVelocity2D = Vector3.zero;

        GameManager.AddTimeController(transform);
    }

    void MyUpdate()
    {
        velocity3D = _myRigid.velocity;

        if (gameObject.layer != _normalLayer)
        {
            if (velocity2D.sqrMagnitude < Mathf.Pow(maxSpeed * 0.7f, 2))
            {
                gameObject.layer = _normalLayer;
            }
        }
        else if(gameObject.layer == _normalLayer && dash)
        {
            dash = false;
        }

    }

    void MyFixedUpdate()
    {
        if (_myRigid.useGravity)
            velocity2D = new Vector3(_myRigid.velocity.x, 0, _myRigid.velocity.z);
        else
            velocity2D = _myRigid.velocity;

        if (eneableDrag)
        {
            _maxVelocity2D = (maxSpeed * Time.fixedDeltaTime * desAceleration * velocity2D.normalized);

            relation = velocity2D.sqrMagnitude / (maxSpeed * maxSpeed);

            if (relation > 1)
                relation = 1;

            _dragFactor = Mathf.Exp(relation * 100 - 100);

            if (_dragFactor < 0.001)
                _dragFactor = 0;

            if (!_move)
            {
                _myRigid.velocity += _maxVelocity2D * -1;
                if (relation < 0.003)
                    _myRigid.velocity = new Vector3(0, _myRigid.velocity.y, 0);

            }
        }

        _move = false;
    }

    void MyStart()
    {
        gameObject.AddTags(Tag.rb);
    }

    protected override void Config()
    {
        MyAwakes += MyAwake;
        MyStarts += MyStart;
        MyUpdates += MyUpdate;
        MyFixedUpdates += MyFixedUpdate;
    }


}

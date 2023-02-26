using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MoveRotAndGlueRb
{

    public FSMEntorno entorno;

    [SerializeField]
    Mesh mesh;

    /// <summary>
    /// collider para detectar el piso
    /// </summary>
    GameObject _cubeDetect;

    MeshCollider _boxCollider;

    public bool eneableDetectFloor
    {
        set
        {
            if (value)
                _layerMask = _originalLayerMask;
            else
                _layerMask = _emptyLayerMask;   
        }
    }

    /// <summary>
    /// Ultima distancia calculada al invocar un isOnFloor
    /// </summary>
    public float lastFloorDistance;
    /*
    {
        get;
        private set;
    }*/

    /// <summary>
    /// Volumen del collider que se va a generar
    /// </summary>
    [Range(0.1f,10)]
    [SerializeField] 
    float _volumeCheckFloor;

    /// <summary>
    /// Layermask para detectar el piso
    /// </summary>
    [SerializeField]
    LayerMask _layerMask;

    LayerMask _originalLayerMask;

    LayerMask _emptyLayerMask;

    public float OnFloor(Collider other)
    {
        if (other.isTrigger || Time.timeScale == 0)
            return lastFloorDistance;

        if (transform.parent == null || glue.transform.parent == null || (glue.transform.parent != null && other.name != glue.transform.parent.name && !other.CompareTag("Enemy")))
            AddGlue(other.transform);

        entorno.CurrentState=entorno.ground;

        lastFloorDistance = 0;

        print(other.name+" colisiono con el player");

        return lastFloorDistance;
    }

    public float FloorDistance()
    {
        foreach (var item in Physics.OverlapSphere(_cubeDetect.transform.position, 0.1f, _layerMask))
        {
            if (!item.gameObject.CompareOneTags(Tag.Player))
            {
                return OnFloor(item);
            }
        }

        //Creo un raycast que va para abajo
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit raycastHit;
        bool chk = Physics.Raycast(ray, out raycastHit, 100, _layerMask);

        //si ese raycast colisiono
        if (chk)
        {
            //print(raycastHit.collider.name + " colisiono");

            //mando un raycast para arriba (buscando a mi objeto asi obtengo la distancia)
            ray = new Ray(raycastHit.point, Vector3.up);

            //lo dibujo
            Debug.DrawRay(raycastHit.point, Vector3.up * raycastHit.distance, Color.blue);

            //ejecuto la instruccion de mandar el raycast
            if(Physics.Raycast(ray, out raycastHit, raycastHit.distance, _layerMask))
            {
                lastFloorDistance = raycastHit.distance;

                /*
                if(lastFloorDistance<0.1f)
                {
                    lastFloorDistance = 0;
                    entorno.SwitchState(entorno.ground);
                }
                */
            }

            //print(raycastHit.distance + " distancia");
        }
        else
            lastFloorDistance = 100;

        return lastFloorDistance;
    }


    protected override void Config()
    {
        base.Config();

        MyAwakes += MyAwake;
     
        MyFixedUpdates += MyFixedUpdate;
    }

    void MyAwake()
    {
        entorno = new FSMEntorno(this);

        _cubeDetect = new GameObject("Detect floor");

        _cubeDetect.transform.SetPositionAndRotation(transform.position + Vector3.down, Quaternion.identity);

        _cubeDetect.transform.localScale = new Vector3(_volumeCheckFloor, 0.1f, _volumeCheckFloor);

        _boxCollider = _cubeDetect.AddComponent<MeshCollider>();

        _cubeDetect.layer = 2;//ignore raycast

        _cubeDetect.transform.parent = transform;

        _boxCollider.sharedMesh = mesh;

        _boxCollider.convex = true;

        _boxCollider.isTrigger = true;

        _originalLayerMask = _layerMask;

    }

    void MyFixedUpdate()
    {
        entorno.UpdateState();

        if (dash)
            return;

        if (transform.position.y < -10)
            transform.position = Vector3.one;
     }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (dash)
            return;

        OnFloor(other);
    }
    */

    private void OnTriggerExit(Collider other)
    {
        entorno.CurrentState=entorno.air;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_cubeDetect!=null)
            Gizmos.DrawSphere(_cubeDetect.transform.position, 0.1f);
    }
}


public class FSMEntorno : FSM<FSMEntorno, Movement>
{
    public BaseEntornoState ground = new BaseEntornoState();
    public BaseEntornoState air = new Air();

    public FSMEntorno(Movement movement): base(movement)
    {
        Init(air);
    }
}

public class BaseEntornoState : IState<FSMEntorno>
{
    public event System.Action<FSMEntorno> onEnter;

    public event System.Action<FSMEntorno> onStay;

    public event System.Action<FSMEntorno> onExit;

    public virtual void OnEnterState(FSMEntorno param)
    {
        onEnter?.Invoke(param);
    }

    public virtual void OnExitState(FSMEntorno param)
    {
        onExit?.Invoke(param);
    }

    public virtual void OnStayState(FSMEntorno param)
    {
        onStay?.Invoke(param);
    }
}

public class Air : BaseEntornoState
{
    public override void OnEnterState(FSMEntorno param)
    {
        param.context.transform.parent = null;
        param.context.glue.transform.parent = null;

        param.context.eneableDrag = false;

        base.OnEnterState(param);
    }

    public override void OnExitState(FSMEntorno param)
    {
        param.context.eneableDrag = true;
        base.OnExitState(param);
    }

    public override void OnStayState(FSMEntorno param)
    {
        param.context.FloorDistance();
        base.OnStayState(param);
    }
}








/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MoveRotAndGlueRb
{

    public FSMEntorno entorno;

    [SerializeField]
    Mesh mesh;

    /// <summary>
    /// collider para detectar el piso
    /// </summary>
    GameObject _cubeDetect;

    MeshCollider _boxCollider;

    public bool eneableDetectFloor
    {
        set
        {
            if (value)
                _layerMask = _originalLayerMask;
            else
                _layerMask = _emptyLayerMask;   
        }
    }

    /// <summary>
    /// Ultima distancia calculada al invocar un isOnFloor
    /// </summary>
    public float lastFloorDistance;

/// <summary>
/// Volumen del collider que se va a generar
/// </summary>
[Range(0.1f, 10)]
[SerializeField]
float _volumeCheckFloor;

/// <summary>
/// Layermask para detectar el piso
/// </summary>
[SerializeField]
LayerMask _layerMask;

LayerMask _originalLayerMask;

LayerMask _emptyLayerMask;

public float OnFloor(Collider other)
{
    if (other.isTrigger || Time.timeScale == 0)
        return lastFloorDistance;

    if (transform.parent == null || glue.transform.parent == null || (glue.transform.parent != null && other.name != glue.transform.parent.name))
        AddGlue(other.transform);

    entorno.SwitchState(entorno.ground);

    lastFloorDistance = 0;

    return lastFloorDistance;
}

public float FloorDistance()
{
    foreach (var item in Physics.OverlapSphere(_cubeDetect.transform.position, 0.1f, _layerMask))
    {
        if (!item.gameObject.CompareOneTags(Tag.Player))
        {
            return OnFloor(item);
        }
    }

    //Creo un raycast que va para abajo
    Ray ray = new Ray(transform.position, Vector3.down);
    RaycastHit raycastHit;
    bool chk = Physics.Raycast(ray, out raycastHit, 100, _layerMask);

    //si ese raycast colisiono
    if (chk)
    {
        //print(raycastHit.collider.name + " colisiono");

        //mando un raycast para arriba (buscando a mi objeto asi obtengo la distancia)
        ray = new Ray(raycastHit.point, Vector3.up);

        //lo dibujo
        Debug.DrawRay(raycastHit.point, Vector3.up * raycastHit.distance, Color.blue);

        //ejecuto la instruccion de mandar el raycast
        if (Physics.Raycast(ray, out raycastHit, raycastHit.distance, _layerMask))
        {
            lastFloorDistance = raycastHit.distance;
        }

        //print(raycastHit.distance + " distancia");
    }
    else
        lastFloorDistance = 100;

    return lastFloorDistance;
}


protected override void Config()
{
    base.Config();

    MyAwakes += MyAwake;

    MyFixedUpdates += MyFixedUpdate;
}

void MyAwake()
{
    entorno = new FSMEntorno(this);

    _cubeDetect = new GameObject("Detect floor");

    _cubeDetect.transform.SetPositionAndRotation(transform.position + Vector3.down, Quaternion.identity);

    _cubeDetect.transform.localScale = new Vector3(_volumeCheckFloor, 0.1f, _volumeCheckFloor);

    _boxCollider = _cubeDetect.AddComponent<MeshCollider>();

    _cubeDetect.layer = 2;//ignore raycast

    _cubeDetect.transform.parent = transform;

    _boxCollider.sharedMesh = mesh;

    _boxCollider.convex = true;

    _boxCollider.isTrigger = true;

    _originalLayerMask = _layerMask;

}

void MyFixedUpdate()
{
    entorno.UpdateState();

    if (dash)
        return;

    if (transform.position.y < -10)
        transform.position = Vector3.one;
}

private void OnTriggerExit(Collider other)
{
    entorno.SwitchState(entorno.air);
}

private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    if (_cubeDetect != null)
        Gizmos.DrawSphere(_cubeDetect.transform.position, 0.1f);
}
}


public class FSMEntorno : FSM<FSMEntorno, Movement>
{
    public BaseEntornoState ground = new BaseEntornoState();
    public BaseEntornoState air = new Air();

    public FSMEntorno(Movement movement) : base(movement)
    {
        Init(air);
    }
}

public class BaseEntornoState : IState<FSMEntorno>
{
    public event System.Action<FSMEntorno> onEnter;

    public event System.Action<FSMEntorno> onStay;

    public event System.Action<FSMEntorno> onExit;

    public virtual void OnEnterState(FSMEntorno param)
    {
        onEnter?.Invoke(param);
    }

    public virtual void OnExitState(FSMEntorno param)
    {
        onExit?.Invoke(param);
    }

    public virtual void OnStayState(FSMEntorno param)
    {
        onStay?.Invoke(param);
    }
}

public class Air : BaseEntornoState
{
    public override void OnEnterState(FSMEntorno param)
    {
        param.context.transform.parent = null;
        param.context.glue.transform.parent = null;

        param.context.eneableDrag = false;

        base.OnEnterState(param);
    }

    public override void OnExitState(FSMEntorno param)
    {
        param.context.eneableDrag = true;
        base.OnExitState(param);
    }

    public override void OnStayState(FSMEntorno param)
    {
        param.context.FloorDistance();
        base.OnStayState(param);
    }
}
*/
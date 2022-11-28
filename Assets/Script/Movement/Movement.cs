using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MoveRotAndGlueRb
{
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
    /// Devuelve true en caso de estar en el suelo
    /// </summary>
    public bool isOnFloor = false;

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

    public void OnFloor(Collider other)
    {
        if (isOnFloor || other.isTrigger || Time.timeScale == 0)
            return;

        if (transform.parent == null || glue.transform.parent == null || (glue.transform.parent != null && other.name != glue.transform.parent.name))
            AddGlue(other.transform);

        lastFloorDistance = 0;

        isOnFloor = true;
    }

    public float FloorDistance()
    {
        foreach (var item in Physics.OverlapSphere(_cubeDetect.transform.position, 0.1f, _layerMask))
        {
            if (!item.gameObject.CompareOneTags(Tag.Player))
            {
                OnFloor(item);
                lastFloorDistance = 0;
                return lastFloorDistance;
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

                if(lastFloorDistance<0.1f)
                {
                    lastFloorDistance = 0;
                    isOnFloor = true;
                }
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
        if (dash)
            return;

        if (transform.position.y < -10)
            transform.position = Vector3.one;

        if (!isOnFloor)
        {
            if (transform.parent != null)
            {
                transform.parent = null;
                glue.transform.parent = null;
            }
            FloorDistance();
        }

     }

    private void OnTriggerEnter(Collider other)
    {
        if (dash)
            return;

        OnFloor(other);
    }

    private void OnTriggerExit(Collider other)
    {
        isOnFloor = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_cubeDetect.transform.position, 0.1f);
    }

}

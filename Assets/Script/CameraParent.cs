using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParent : MonoBehaviour
{
    /// <summary>
    /// Jugador
    /// </summary>
    public Player_Character character;

    /// <summary>
    /// referencia la camara principal
    /// </summary>
    public Camera cam;

    /// <summary>
    /// objeto para la referencia visual se eliminara desp
    /// </summary>
    public GameObject sphere;

    /// <summary>
    /// vector posicion que guarda donde colisiono el raycast de la camara
    /// </summary>
    public Vector3 hitPoint;

    /// <summary>
    /// vector auxiliar para hacer calculos de la posicion actual de la camara
    /// </summary>
    public Vector3 offSetPos;

    /// <summary>
    /// el offset de la camara, con respecto al padre
    /// </summary>
    public Vector3 offSet;

    /// <summary>
    /// el offset que se le sumara a la camara, con respecto al padre
    /// </summary>
    public Vector3 offSetZoom;

    /// <summary>
    /// el offset que se le sumara a la camara, con respecto al padre, a modo de no chocar contra las paredes
    /// </summary>
    public Vector3 offSetFix;


    /// <summary>
    /// Vector que contiene los angulos de euler a los que se desea girar la camara
    /// </summary>
    [SerializeField]
    Vector3 _axis;

    /// <summary>
    /// Debido a que unity es un extraño ser, los angulos que devuelve son entre 0 y 360, lo que nos dificulta calcular lerps, 
    /// por eso lo trabajamos en paralelo
    /// </summary>
    Vector3 _eulerAngles;


    /// <summary>
    /// el vector direccion del offset
    /// </summary>
    Vector3 _offsetNormalize;

    /// <summary>
    /// Distancia maxima para detectar una colision
    /// </summary>
    float _distance;

    [Range(1, 50)]
    [SerializeField]
    float _linearVelocity;

    [Range(0.01f, 1)]
    [SerializeField]
    float _sensibilityRelationVelocity;

    [SerializeField]
    [Range(1, 100)]
    float _sensibility;


    [SerializeField]
    float _rayDetection;

    [SerializeField]
    LayerMask _layerMask;

    [SerializeField]
    LayerMask _layerMaskCameraNotPass;

    #region funciones
    /// <summary>
    /// Rota la camara tomando en cuenta la sensibilidad del mouse asi como una velocidad angular, para dar un movimiento fluido
    /// </summary>
    /// <param name="axis">Vector 2 que recibe el desplazamiento en grados en x e y</param>
    public void Rotate(Vector2 axis)
    {
        axis *= _sensibility;

        float zy = _axis.x + axis.y;
        float zx = _axis.y + axis.x;

        // || (axis.y<0&&dontDown)
        if (zy > 90 || zy<-90)
        {
            zy -= axis.y;
        }


        if (zy > 360)
        {
            zy -= 360;
        }
        else if (zy < -360)
        {
            zy += 360;
        }


        if (zx >= 360)
        {
            zx -= 360;
            _eulerAngles.y -= 360;
        }
        else if(zx <= -360)
        {
            zx += 360;
            _eulerAngles.y += 360;
        }

        _axis = new Vector3(zy, zx, 0);
        
    }

    /// <summary>
    /// Define el offset de la camara, tomando en cuenta un movimiento fluido definido por _linearVelocity
    /// </summary>
    /// <param name="offset">Vector 3 que es la distancia del character</param>
    public void OffSet(Vector3 offset)
    {
        offSet = offset;
        _offsetNormalize = offset.normalized;
        _distance = offset.magnitude;
    }

    /// <summary>
    /// Modifica el offset acual sumandole el vector dado, con un movimiento fluido definido por _linearVelocity
    /// </summary>
    /// <param name="movement">Vector 3 que define cuanto quiere mover la camara, siendo los ejes x para el horizontal, y para el vertical, y z para la profundidad</param>
    public void MoveCamera(Vector3 movement)
    {
        OffSet(movement+offSet);
    }

    /// <summary>
    /// Define el vector de zoom, el cual se le suma a la camara
    /// </summary>
    /// <param name="zoom"></param>
    public void ZoomIn(Vector3 zoom)
    {
        offSetZoom = zoom;
    }

    /// <summary>
    /// Define en cero el vector de zoom
    /// </summary>
    public void ZoomOut()
    {
        offSetZoom = Vector3.zero;
    }

    /// <summary>
    /// Funcion encargada de mover horizontalmente la camara
    /// </summary>
    public bool Flip()
    {
        offSetZoom = new Vector3(-offSetZoom.x, offSetZoom.y, offSetZoom.z); 
        OffSet(new Vector3(_offsetNormalize.x * -1, _offsetNormalize.y, _offsetNormalize.z) * _distance);

        return false;
    }

    public void Speed(float f, float m=-1)
    {
        _sensibility = f;
        if (m > 0)
            _sensibilityRelationVelocity = m;
    }

    #endregion

    #region funciones de unity

    private void Start()
    {
        //offSet = transform.GetChild(0).position - character.transform.position;

        OffSet(offSet);

        _eulerAngles = transform.rotation.eulerAngles;

        cam = GetComponentInChildren<Camera>();

        StartCoroutine(CameraPan());

        GameManager.AddTimeController(cam.transform);

        GameManager.AddTimeController(transform);
        
        offSetPos = offSet;

    }

    private void LateUpdate()
    {

        transform.position = character.transform.position;

        if (Quaternion.Euler(_axis) != transform.rotation)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(_eulerAngles, _axis, Time.deltaTime * (_sensibility * _sensibilityRelationVelocity)));
            _eulerAngles=Vector3.Lerp(_eulerAngles, _axis, Time.deltaTime * (_sensibility * _sensibilityRelationVelocity));
        }
            
        if (Controllers.eneable && (transform.GetChild(0).localPosition + offSetFix) != (offSet + offSetZoom))
        {

            offSetPos = Vector3.Lerp(offSetPos, (offSet + offSetZoom), Time.deltaTime * _linearVelocity);

            transform.GetChild(0).localPosition = offSetPos - offSetFix;
        }
         
               

        Vector3 camaraPos = new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 2, 0);

        Ray ray = Camera.main.ScreenPointToRay(camaraPos);

        Debug.DrawRay(transform.GetChild(0).position, ray.direction, Color.red);

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, _layerMask))
        {
            hitPoint = raycastHit.point;

            if (raycastHit.distance < _rayDetection)
            {

                float distance = 50 * raycastHit.distance / _rayDetection;

                character.scopedPoint = hitPoint;

                character.scoped = raycastHit.collider;

                
                sphere.transform.position = hitPoint;
                sphere.transform.localScale = Vector3.one * distance;

                Collider[] col = Physics.OverlapSphere(hitPoint, distance, _layerMask);

                Collider interact = null;

                foreach (Collider item in col)
                {
                    if (item.CompareTag("Interactuable") && (item.transform.position-hitPoint).sqrMagnitude < distance*distance)
                    {
                        //print(item.name);
                        interact = item;
                        distance = (item.transform.position - hitPoint).magnitude;
                    }
                }

                if (interact != null && interact.CompareTag("Interactuable"))
                    character.interactuable = interact.GetComponent<Interactuable_LogicActive>();
                else
                    character.interactuable=null;
            }
        }

        ray = new Ray(character.transform.position,transform.rotation*(offSetPos*1.1f));

        Debug.DrawRay(character.transform.position, ray.direction, Color.white);

        if (Physics.Raycast(ray, out raycastHit, _distance, _layerMaskCameraNotPass))
        {
            if ((raycastHit.point - transform.position).sqrMagnitude < _distance * _distance)
            {
                float d = (raycastHit.point - transform.position).magnitude;

                float proporcionalDistance = (1 - (cam.transform.localPosition.sqrMagnitude / (_distance * _distance)));

                offSetFix = offSetPos.normalized*_distance - _offsetNormalize * (d- 0.5f) - Vector3.up*0.5f* proporcionalDistance;
                
                if(d < _distance / 1.5f)
                {
                    Vector3 position = new Vector3(_offsetNormalize.x * -1, _offsetNormalize.y, _offsetNormalize.z) * _distance;
                    
                    ray = new Ray(character.transform.position, transform.TransformDirection(position));
                    Debug.DrawRay(character.transform.position, ray.direction, Color.black);
                    if (!Physics.Raycast(ray, out raycastHit, _distance, _layerMask))
                    {
                        offSetFix = Vector3.zero;
                        character.Flip();
                    }
                }
            }
        }
        else
        {
            offSetFix = Vector3.zero;
        }
    }
    #endregion

    IEnumerator CameraPan()
    {
        Timer myTimer = Timers.Create(1);
        
        while (!myTimer.Chck())
        {
            yield return null;
        }
        

        float linearAux = _linearVelocity;

        _linearVelocity = 0.3f;

        while(cam.transform.localRotation!=Quaternion.identity && Mathf.Abs((cam.transform.localPosition-offSet).sqrMagnitude)>1)
        {
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition + offSetFix, offSet + offSetZoom, Time.deltaTime * _linearVelocity) - offSetFix;
            offSetPos = transform.GetChild(0).localPosition;
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.identity, Time.deltaTime);
            yield return null;
        }

        _linearVelocity = linearAux;

        Timers.Destroy(myTimer);

        Controllers.eneable = true;
    }
}



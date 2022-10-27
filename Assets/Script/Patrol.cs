using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Patrol
{

    /// <summary>
    /// setea si reinicia la patrulla desde 0, o la recorre una vez terminada en orden inverso
    /// </summary>
    public bool reverse;

    /// <summary>
    /// Indice del array de la patrulla
    /// </summary>
    public int iPatrulla = 0;

    /// <summary>
    /// lista que contiene todos los puntos de la patrulla
    /// </summary>
    public List<Transform> patrol;


    /// <summary>
    /// variable para chequear si es la primera vez que que entra en el timer
    /// </summary>
    public bool firstTime
    {
        get;
        private set;
    }

    /// <summary>
    /// vector de distancia que falta para llegar al objetivo
    /// </summary>
    [SerializeField]
    Vector3 _distance;

    /// <summary>
    /// privada que se encarga de chequear si tiene q avanzar en la lista o no
    /// </summary>
    bool _reverseEffect;

    /// <summary>
    /// Cuanto se desea esperar en cada punto
    /// </summary>
    [SerializeField]
    float _waitTime;

    /// <summary>
    /// referencia privada del monobehabior que le creara
    /// </summary>
    MonoBehaviour _mono;

    /// <summary>
    /// Timer que espera una vez alcanzado el destino, para setear el siguiente
    /// </summary>
    [SerializeField]
    Timer _wait;


    /// <summary>
    /// calcula la distancia hasta el numero (pero no lo guarda) punto de patrullaje
    /// </summary>
    /// <returns>retorna el vector de distancia</returns>
    public Vector3 Distance(int i)
    {
        return patrol[i].position - _mono.transform.position;
    }

    /// <summary>
    /// calcula la distancia hasta el siguiente punto de patrullaje
    /// </summary>
    /// <returns>retorna el vector de distancia</returns>
    public Vector3 Distance()
    {
        _distance = patrol[iPatrulla].position - _mono.transform.position;
        return _distance;
    }

    /// <summary>
    /// devuelve el siguiente punto del patrullaje
    /// </summary>
    /// <param name="reverseEffect">devuelve si retrocede o no</param>
    /// <returns>indice del patrullaje</returns>
    public int NextPoint(out bool reverseEffect)
    {
        int i = iPatrulla;

        reverseEffect = _reverseEffect;

        if (!reverseEffect)
            i++;
        else
            i--;

        if (i >= patrol.Count)
        {
            if (reverse)
            {
                reverseEffect = !reverseEffect;
                i -= 2;
            }
            else
                i = 0;
        }
        else if (i <= -1)
        {
            i = 0;

            reverseEffect = !reverseEffect;
        }

        return i;
    }



    /// <summary>
    /// chequea si se llego a la distancia minima, y esperara a un timer para ir setear el siguiente punto de patrullaje
    /// </summary>
    /// <param name="minimal"></param>
    /// <returns>En caso de llegar a la distancia minima devolvera un true, por lo contrario un false</returns>
    public bool MinimalChck(float minimal, bool automatic=true)
    {
        if (_distance.sqrMagnitude < minimal * minimal)
        {
            if (_wait.Chck() && !firstTime)
            {
                if (automatic)
                {
                    iPatrulla = NextPoint(out _reverseEffect);
                }
            }
            else if (_wait.Chck() && firstTime)
            {
                _wait.Reset();
                firstTime = false;
            }

            Distance();
            return true;
        }
        if(_wait.Chck())
            firstTime = true;
        return false;
    }

    // Start que configura todo lo necesario para usar la clase
    public void Start(MonoBehaviour m)
    {
        _mono = m;

        _wait = Timers.Create(_waitTime);

        if (patrol.Count <= 0)
        {
            GameObject aux = new GameObject(_mono.name + " position");
            aux.transform.position = _mono.transform.position;
            patrol.Add(aux.transform);
        }
    }
}


public interface IPatrolReturn
{
    Patrol PatrolReturn();
}
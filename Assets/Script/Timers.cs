using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timers : MonoBehaviour
{

    static List<Timer> timersList = new List<Timer>();

    /// <summary>
    /// Crea un timer que se almacena en una lista para restarlos de forma automatica
    /// </summary>
    /// <param name="totTime2">el tiempo que dura el contador</param>
    /// <param name="m">el multiplicador del contador</param>
    /// <returns>Devuelve la referencia del contador creado</returns>
    public static Timer Create(float totTime2 = 10, float m = 1)
    {
        Timer newTimer = new Timer(totTime2, m);
        timersList.Add(newTimer);
        return newTimer;
    }

    /// <summary>
    /// Destruye un timer de la lista
    /// </summary>
    /// <param name="timy">El timer que sera destruido</param>
    public static void Destroy(Timer timy)
    {
        timersList.Remove(timy);
    }

    void Update()
    {
        for (int i = 0; i < timersList.Count; i++)
        {
            timersList[i].Substract(Time.deltaTime);
        }
    }
}

[System.Serializable]
public class Timer
{
    [SerializeField]
    float _totalTime;

    [SerializeField]
    float _currentTime;

    float _multiply;
    bool _freeze;

    /// <summary>
    /// Modifica el numero que multiplica la constante temporal, y asi acelerar o disminuir el timer
    /// </summary>
    /// <param name="m">Por defecto es 1</param>
    public void Multiply(float m)
    {
        _multiply = m;
    }
    /// <summary>
    /// En caso de que el contador este detenido lo reanuda
    /// </summary>
    public void Start()
    {
        _freeze = true;
    }

    /// <summary>
    /// Frena el contador, no resetea ni modifica el contador actual
    /// </summary>
    public void Stop()
    {
        _freeze = false;
    }
    /// <summary>
    /// Setea el contador, y comienza la cuenta (si se quiere) desde ese numero
    /// </summary>
    /// <param name="totalTim">El numero a contar</param>
    /// <param name="f">Si arranca a contar o no</param>
    public void Set(float totalTim, bool f=true)
    {
        _totalTime = totalTim;
        _freeze = f;
        Reset();
    }

    /// <summary>
    /// Chequea si el contador llego a su fin
    /// </summary>
    /// <returns>Devuelve true si llego a 0</returns>
    public bool Chck()
    {
        if (_currentTime<=0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Reinicia el contador a su valor por defecto, para reiniciar la cuenta
    /// </summary>
    public void Reset()
    {
        _currentTime = _totalTime;
    }
    /// <summary>
    /// Efectua una resta en el contador
    /// </summary>
    /// <param name="n">En caso de ser negativo(-) suma al contador, siempre y cuando no este frenado</param>
    public void Substract(float n)
    {
        if (_currentTime > 0 && _freeze)
        {
            _currentTime -= n*_multiply;
        }
    }
    /// <summary>
    /// Configura el timer para su uso
    /// </summary>
    /// <param name="totTim">valor por defecto a partir de donde se va a contar</param>
    /// <param name="m">Modifica el multiplicador del timer, por defecto 0</param>
    public Timer(float totTim = 10, float m=1)
    {
        _multiply = m;
        Start();
        Set(totTim);
    }
}
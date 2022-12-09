using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimersManager : MonoBehaviour
{
    List<Timer> timersList;

    static TimersManager instance;





    /// <summary>
    /// Crea un timer que se almacena en una lista para restarlos de forma automatica
    /// </summary>
    /// <param name="totTime2">el tiempo que dura el contador</param>
    /// <param name="m">el multiplicador del contador</param>
    /// <returns>Devuelve la referencia del contador creado</returns>
    public static Timer Create(float totTime2 = 10, float m = 1)
    {
        Timer newTimer = new Timer(totTime2, m);
        instance.timersList.Add(newTimer);
        return newTimer;
    }

    /// <summary>
    /// Crea una rutina que ejecutara una funcion al cabo de un tiempo
    /// </summary>
    /// <param name="totTime">el tiempo total a esperar</param>
    /// <param name="action">la funcion que se ejecutara</param>
    /// <param name="destroy">si se destruye luego de ejecutar la funcion</param>
    /// <returns>retorna la rutina creada</returns>
    public static Routine Create(float totTime, Action action, bool destroy=true)
    {
        Routine newTimer = new Routine(totTime, action, destroy);
        instance.timersList.Add(newTimer);
        return newTimer;
    }

    public static CompleteRoutine Create(float totTime, Action start, Action update, Action end, bool destroy = true)
    {
        CompleteRoutine newTimer = new CompleteRoutine(totTime, start, update, end, destroy);
        instance.timersList.Add(newTimer);
        return newTimer;
    }

    /// <summary>
    /// Destruye un timer de la lista
    /// </summary>
    /// <param name="timy">El timer que sera destruido</param>
    public static void Destroy(Timer timy)
    {
        instance.timersList.Remove(timy);
    }


    private void Awake()
    {
        timersList = new List<Timer>();
        instance = this;

        print("Se instancio el timer");
    }

    void Update()
    {
        for (int i = 0; i < timersList.Count; i++)
        {
            timersList[i].Substract(Time.deltaTime);

            if (timersList[i].Chck && timersList[i] is Routine && ((Routine)timersList[i]).execute)
            {
                if (((Routine)timersList[i]).Execute())
                    timersList.RemoveAt(i);
            }
        }
    }
}



public class Tim : IGetPercentage
{
    [SerializeField]
    protected float _totalTime;

    [SerializeField]
    protected float _currentTime;

    /// <summary>
    /// Reinicia el contador a su valor por defecto, para reiniciar la cuenta
    /// </summary>
    public virtual void Reset()
    {
        _currentTime = _totalTime;
    }

    /// <summary>
    /// Efectua una resta en el contador
    /// </summary>
    /// <param name="n">En caso de ser negativo(-) suma al contador, siempre y cuando no este frenado</param>
    public virtual float Substract(float n)
    {
        if (_currentTime > 0)
        {
            _currentTime -= n;
        }

        return Percentage();
    }

    /// <summary>
    /// Setea el contador
    /// </summary>
    /// <param name="totalTim">El numero a contar</param>
    public void Set(float totalTim)
    {
        _totalTime = totalTim;
        Reset();
    }

    public float Percentage()
    {
        return 1 - (_currentTime) / _totalTime;
    }

    public Tim(float totTim = 10)
    {
        Set(totTim);
    }
}


[System.Serializable]
public class Timer : Tim
{
    float _multiply;
    bool _freeze;

    public bool pauseTimer
    {
        set
        {
            if (value)
                Stop();
            else
                Start();
        }
    }

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
        base.Set(totalTim);
        _freeze = f;
    }

    /// <summary>
    /// Chequea si el contador llego a su fin
    /// </summary>
    /// <returns>Devuelve true si llego a 0</returns>
    public bool Chck
    {
        get
        {
            return _currentTime <= 0;
        }
        
    }


    /// <summary>
    /// Efectua una resta en el contador
    /// </summary>
    /// <param name="n">En caso de ser negativo(-) suma al contador, siempre y cuando no este frenado</param>
    public override float Substract(float n)
    {
        if (_currentTime > 0 && _freeze)
        {
            _currentTime -= n*_multiply;
        }

        return Percentage();
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

/// <summary>
/// rutina que ejecutara una accion desp de que termine el tiemer
/// </summary>
[System.Serializable] 
public class Routine : Timer
{    
    public Action action;

    public bool destroy;

    public bool execute;

    public override void Reset()
    {
        base.Reset();
        execute = true;
    }

    public bool Execute()
    {
        action();
        execute = false;
        return destroy;
    }

    public Routine(float timer, Action action, bool destroy = true) : base(timer)
    {
       
        this.action = action;
        this.destroy = destroy;
        execute = true;
    }


}

/// <summary>
/// rutina que ejecutara una funcion al comenzar/reiniciar, otra en cada frame, y otra al final
/// </summary>
[System.Serializable]
public class CompleteRoutine : Routine
{
    Action start;
    Action update;
    public bool pauseRoutine;

    public override void Reset()
    {
        base.Reset();
        start?.Invoke();
    }

    /// <summary>
    /// funcion que ejecutara de forma automatica cada frame
    /// </summary>
    public override float Substract(float n)
    {
        if (!pauseRoutine)
        {
            base.Substract(n);
            update();
        }

        return Percentage();
    }

    /// <summary>
    /// crea una rutina que ejecutara una funcion al comenzar/reiniciar, otra en cada frame, y otra al final
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="start"></param>
    /// <param name="update"></param>
    /// <param name="end"></param>
    /// <param name="destroy"></param>
    public CompleteRoutine(float timer, Action start, Action update, Action end, bool destroy = true) : base(timer, end, destroy)
    {
        this.start = start;
        this.update = update;
        start?.Invoke();
    }
}


public interface IGetPercentage
{
    float Percentage();
}
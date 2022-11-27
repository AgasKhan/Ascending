using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timers : MonoBehaviour
{
    List<Timer> timersList;
    List<Routine> routines;
    List<CompleteRoutine> completeRoutines;

    static Timers instance;

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
        instance.routines.Add(newTimer);
        return newTimer;
    }

    public static CompleteRoutine Create(float totTime, Action start, Action update, Action end, bool destroy = true)
    {
        CompleteRoutine newTimer = new CompleteRoutine(totTime, start, update, end, destroy);
        instance.completeRoutines.Add(newTimer);
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
        routines = new List<Routine>();
        completeRoutines = new List<CompleteRoutine>();
        instance = this;

        print("Se instancio el timer");
    }

    void Update()
    {
        for (int i = 0; i < timersList.Count; i++)
        {
            timersList[i].Substract(Time.deltaTime);
        }

        for (int i = routines.Count-1; i >= 0; i--)
        {
            routines[i].timer.Substract(Time.deltaTime);
            if(routines[i].finish && routines[i].execute)
            {
                if (routines[i].Execute())
                    routines.RemoveAt(i);
            }
        }

        for (int i = completeRoutines.Count - 1; i >= 0; i--)
        {
            completeRoutines[i].Update();
            if (completeRoutines[i].finish && completeRoutines[i].execute)
            {
                if (completeRoutines[i].Execute())
                    completeRoutines.RemoveAt(i);
            }
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

/// <summary>
/// rutina que ejecutara una accion desp de que termine el tiemer
/// </summary>
[System.Serializable] 
public class Routine
{
    public Timer timer;
    
    public Action action;

    public bool destroy;

    public bool execute;

    public bool pauseTimer
    {
        set 
        {
            if(value)
                timer.Stop();
            else
                timer.Start();
        }
    }
    
    public bool finish
    {
        get
        {
            return timer.Chck();
        }
    }

    public virtual void Restart()
    {
        timer.Reset();
        execute = true;
    }

    public bool Execute()
    {
        action();
        execute = false;
        return destroy;
    }

    public Routine(float timer, Action action, bool destroy = true)
    {
        this.timer = new Timer(timer);
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

    public override void Restart()
    {
        base.Restart();
        start?.Invoke();
    }

    /// <summary>
    /// funcion que ejecutara de forma automatica cada frame
    /// </summary>
    public void Update()
    {
        if(!pauseRoutine)
        {
            timer.Substract(Time.deltaTime);
            update();
        }
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
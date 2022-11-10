using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MyScripts : MonoBehaviour
{
    /*
    abstract protected void MyAwake();
    abstract protected void MyStart();
    abstract protected void MyUpdate();
    abstract protected void MyFixedUpdate();
    */

    protected event Action MyAwakes;
    protected event Action MyStarts;
    protected event Action MyUpdates;
    protected event Action MyFixedUpdates;


    protected abstract void Config();

    internal void Awake()
    {
        Config();

        MyAwakes.Invoke();
    }

    // Update is called once per frame
    internal void Update()
    {
        MyUpdates?.Invoke();
    }

    internal void FixedUpdate()
    {
        MyFixedUpdates?.Invoke();
    }

    internal void Start()
    {
        MyStarts?.Invoke();
    }

}


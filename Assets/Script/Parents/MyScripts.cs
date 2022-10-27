using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyScripts : MonoBehaviour
{

    abstract protected void MyAwake();
    abstract protected void MyStart();
    abstract protected void MyUpdate();
    abstract protected void MyFixedUpdate();

    void Awake()
    {
        MyAwake();
    }

    // Update is called once per frame
    void Update()
    {
        MyUpdate();
    }

    void FixedUpdate()
    {
        MyFixedUpdate();
    }

    private void Start()
    {
        MyStart();
    }


}


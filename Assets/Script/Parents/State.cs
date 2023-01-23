using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T>
{
    public System.Action<T> on;
    public System.Action<T> update;
    public System.Action<T> off;
}

public interface IState<T>
{
    void On(T param);

    void Update(T param);

    void Off(T param);
}




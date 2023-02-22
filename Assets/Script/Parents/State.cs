using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    void OnEnterState(T param);

    void OnStayState(T param);

    void OnExitState(T param);
}

public interface IState
{
    void OnEnterState();

    void OnStayState();

    void OnExitState();
}

public interface ISwitchState<T>
{
    void SwitchState(IState<T> state);

    IState<T> ReturnState();
}


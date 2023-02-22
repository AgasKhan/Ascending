using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM<T, Context> : ISwitchState<T> where T : FSM<T,Context>
{

    public Context context;

    IState<T> currentState;

    T FSMConvertToChild()
    {
        return (T)this;
    }

    public IState<T> ReturnState()
    {
        return currentState;
    }

    public void SwitchState(IState<T> state)
    {
        if (state == currentState || state== null)
            return;

        currentState.OnExitState(FSMConvertToChild());
        Init(state);
    }

    public void UpdateState()
    {
        currentState.OnStayState(FSMConvertToChild());
    }

    protected void Init(IState<T> first)
    {
        currentState = first;
        currentState.OnEnterState(FSMConvertToChild());
    }



    protected FSM(Context reference)
    {
        this.context = reference;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T1, T2> where T2 : MonoBehaviour
{
    private T2 owner;
    private Dictionary<T1, State<T2>> states;
    private Coroutine curUpdateCoroutine;

    private State<T2> curState;

    public StateMachine(T2 owner)
    {
        this.owner = owner;
        curState = null;
        curUpdateCoroutine = null;
        states = new Dictionary<T1, State<T2>>();
    }

    public void AddState(T1 type, State<T2> state)
    {
        states.Add(type, state);
    }
    public void ChangeState(T1 type)
    {
        //if (curState == states[type])
        //    return;

        if (curUpdateCoroutine != null)
            owner.StopCoroutine(curUpdateCoroutine);

        curState?.OnStateExit(owner);

        curState = states[type];

        curState.OnStateEnter(owner);
        curUpdateCoroutine = owner.StartCoroutine(curState.OnStateUpdate(owner));
    }
}

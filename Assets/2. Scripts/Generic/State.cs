using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    public abstract void OnStateEnter(T owner);
    public abstract IEnumerator OnStateUpdate(T owner);
    public abstract void OnStateExit(T owner);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FindTargetOfOverlapSphere))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public abstract class Monster<T1, T2> : MonoBehaviour, ITargetable, IDamable  where T2 : MonoBehaviour
{
    [SerializeField] protected GameObject targetedObject = null;
    [SerializeField] protected GameObject target;
    [SerializeField] protected int hp;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float rotSpeed = 20f;

    protected Animator animator;
    protected CharacterController controller;

    protected StateMachine<T1, T2> stateMachine;
    protected FindTargetOfOverlapSphere findTarget;

    public void ChangeState(T1 nextState)
    {
        stateMachine.ChangeState(nextState);
    }

    protected void FindTarget()
    {
        Collider[] coll = null;
        if (!findTarget.FindTarget(ref coll))
        {
            target = null;
            return;
        }

        target = coll[0].gameObject;
    }

    public void NonTarget()
    {
        target?.SetActive(false);
    }

    public void OnTarget()
    {
        target?.SetActive(true);
    }

    public virtual void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public void Initialize()
    {
        Debug.Log("Monster Awake");
        //targetedObject?.SetActive(false);
        findTarget = GetComponent<FindTargetOfOverlapSphere>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
}

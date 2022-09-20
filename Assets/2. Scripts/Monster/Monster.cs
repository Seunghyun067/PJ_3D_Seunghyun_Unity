using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FindTargetOfOverlapSphere))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public abstract class Monster<T1, T2> : MonoBehaviour, ITargetable, IDamable  where T2 : MonoBehaviour
{
    [SerializeField] protected GameObject targetedObject = null;
    [SerializeField] protected int hp;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float rotSpeed = 20f;

    [SerializeField] protected Renderer[] myRenderer;
    protected GameObject target;
    protected Animator animator;
    protected CharacterController controller;

    protected StateMachine<T1, T2> stateMachine;
    protected FindTargetOfOverlapSphere findTarget;
    protected bool isDead = false;

    protected IEnumerator DissolveEnable(float timeScale = 1f)
    {
        float dissolveValue = 1f;

        while (dissolveValue > 0f)
        {
            for (int i = 0; i < myRenderer.Length; ++i)
                myRenderer[i].material.SetFloat("_Dissolve", dissolveValue -= Time.deltaTime * timeScale);
            yield return null;
        }
        for (int i = 0; i < myRenderer.Length; ++i)
            myRenderer[i].material.SetFloat("_Dissolve", 0f);
        yield return null;
    }
    protected IEnumerator DissolveDisable(float timeScale = 1f)
    {
        float dissolveValue = 0f;

        while (dissolveValue < 1f)
        {
            for (int i = 0; i < myRenderer.Length; ++i)
                myRenderer[i].material.SetFloat("_Dissolve", dissolveValue += Time.deltaTime * timeScale);
            yield return null;
        }
        for (int i = 0; i < myRenderer.Length; ++i)
            myRenderer[i].material.SetFloat("_Dissolve", 1f);
        Destroy(gameObject);
        yield return null;
    }

    public void ChangeState(T1 nextState)
    {
        stateMachine.ChangeState(nextState);
    }

    protected void FindTarget()
    {
        Collider[] colls = null;

        if (!findTarget.FindTarget(ref colls))
        {
            target = null;
            return;
        }

        foreach(var coll in colls)
        {
            int rayerNumber = coll.gameObject.layer;

            if (rayerNumber == LayerMask.NameToLayer("Player"))
            {
                target = coll.gameObject;
                break;
            }
            else if(rayerNumber == LayerMask.NameToLayer("Monster"))
            {
                target = coll.gameObject.GetComponent<SwordRobot>().target;

                if(target)
                    break;
                continue;
            }
            
        }

        
    }

    public void NonTarget()
    {
        if (isDead) return;

        targetedObject?.SetActive(false);
    }

    public void OnTarget()
    {
        if (isDead) return;
        targetedObject?.SetActive(true);
    }

    public virtual void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public void Initialize()
    {
        Debug.Log("Monster Awake");
        targetedObject?.SetActive(false);
        findTarget = GetComponent<FindTargetOfOverlapSphere>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public bool IsTarget()
    {
        return !isDead;
    }
}

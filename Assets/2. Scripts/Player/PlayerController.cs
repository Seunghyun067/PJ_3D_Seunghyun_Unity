using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int comboAttackCount { get; set; } = 0;

    [SerializeField] private TrailRenderer katanaTrail;
    [SerializeField] public Collider katanaCollider;

    [SerializeField] public Transform camTransform;

    [SerializeField] public float moveSpeed { get; }= 5;
    [SerializeField] public float rotSpeed { get; } = 5;
    [SerializeField] public int attackDamage { get; } = 5;

    private ITargetable attackTarget;
    private Transform targetTransform = null;
    public Transform TargetTransform { get { return targetTransform; } }
    private FindTargetOfOverlapSphere findTarget;
    private Coroutine distorCo;

    public void KatanaTrailActive(bool isActive)
    {        
        katanaTrail.enabled = isActive;
    } 


    ITargetable preTarget;
    void FindTarget()
    {
        Collider[] colls = findTarget.FindTarget();

        if (0 == colls.Length)
        {
            preTarget?.NonTarget();
            attackTarget = preTarget = null;
            targetTransform = null;
            return;
        }

        ITargetable target = null;
        targetTransform = null;
        foreach(var coll in colls)
        {
            target = coll.GetComponent<ITargetable>();

            if (!target.IsTarget())
                continue;
            else
            {
                targetTransform = coll.gameObject.transform;
                attackTarget = target;
            }
        }

        if (attackTarget == preTarget)
            return;

        preTarget?.NonTarget();
        attackTarget.OnTarget();
        preTarget = attackTarget;
    }
    void Awake()
    {
        if (katanaTrail)
        {
            KatanaTrailActive(false);
            //moveTrail.SetActive(true);
        }
        katanaCollider.enabled = false;
        findTarget = GetComponent<FindTargetOfOverlapSphere>();
    }

    private void FixedUpdate()
    {
        FindTarget();
    } 

}

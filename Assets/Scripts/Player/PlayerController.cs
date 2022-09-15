using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int comboAttackCount { get; set; } = 0;

    [SerializeField] public TrailRenderer katanaTrail;
    [SerializeField] public TrailRenderer katanaTrail2;
    [SerializeField] public Collider katanaCollider;

    [SerializeField] public Transform camTransform;

    [SerializeField] private Transform bodyPosition;
    [SerializeField] private float findTargetDst = 5;
    [SerializeField] private LayerMask findTargetLayerMask;

    [SerializeField] public float moveSpeed { get; }= 5;
    [SerializeField] public float rotSpeed { get; } = 5;
    [SerializeField] public int attackDamage { get; } = 5;

    public Monster attackTarget { get; set; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bodyPosition.position, findTargetDst);
    }


    Monster preTarget;
    void FindTarget()
    {        
        Collider[] coll = Physics.OverlapSphere(bodyPosition.position, findTargetDst, findTargetLayerMask);
               
        if (0 == coll.Length)
        {
            preTarget?.GetComponent<ITargetable>().NonTarget();
            attackTarget = preTarget = null;
            return;
        }

        attackTarget = coll[0].GetComponent<Monster>();
 

        if(attackTarget == preTarget)
            return;

        preTarget?.GetComponent<ITargetable>().NonTarget();
        attackTarget.GetComponent<ITargetable>().OnTarget();
        preTarget = attackTarget;
    }
    void Start()
    {
        if (katanaTrail)
        {
            katanaTrail.enabled = false;
            katanaTrail2.enabled = false;
        }

        Debug.Log("OK");
    }

    private void FixedUpdate()
    {
        FindTarget();
    }
}

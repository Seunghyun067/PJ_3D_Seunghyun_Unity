using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamable
{
    private int comboAttackCount { get; set; } = 0;

    //[SerializeField] private TrailRenderer katanaTrail;
    //[SerializeField] public Collider katanaCollider;

    [SerializeField] public Transform camTransform;

    [SerializeField] public float moveSpeed { get; }= 5;
    [SerializeField] public float rotSpeed { get; } = 5;
    [SerializeField] public int attackDamage { get; } = 5;
    [SerializeField] private int hp = 10;

    private ITargetable attackTarget;
    private Transform targetTransform = null;
    public Transform TargetTransform { get { return targetTransform; } }
    private FindTargetOfOverlapSphere findTarget;
    private Coroutine distorCo;
    private Animator animator;

    public Action parringAction;
    [HideInInspector] public Katana katana;

    public GameObject[] BloodFX;

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
        katana = GetComponentInChildren<Katana>();        
        findTarget = GetComponent<FindTargetOfOverlapSphere>();
        animator = GetComponent<Animator>();
    }

    public void ParryAttackGo()
    {
        animator.SetTrigger("ParryAttack");        
    }

    private void FixedUpdate()
    {
        FindTarget();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        
        animator.SetTrigger("Hit");
        Vector3 pos = transform.position;
        pos.y += 1f;
        Instantiate(BloodFX[UnityEngine.Random.Range(1, BloodFX.Length)], pos, Quaternion.identity);
        katana.KatanaTrailActive(false);
        katana.AttackColliderActive(false);
    }
}

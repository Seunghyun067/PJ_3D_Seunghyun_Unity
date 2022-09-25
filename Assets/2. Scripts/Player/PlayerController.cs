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
    [SerializeField] private Transform bodyPoint;
    public Vector3 BodyPoint { get { return bodyPoint.position; } }

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
            target = coll.GetComponentInParent<ITargetable>();

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
    [SerializeField] private Transform bossCo;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var laser = ObjectPooling.Instance.PopObject("RedLaser");
            Vector3 pos = Camera.main.transform.position - Camera.main.transform.forward * 10f;
            pos.y -= 1f;
            laser.transform.position = pos;
            laser.GetComponent<BossShootLaser>().LaserShoot(bossCo.position);

            
        }
    }

    private void FixedUpdate()
    {
        FindTarget();
    }

    public void HitTrigger(string triggerTag)
    {
        animator.SetTrigger(triggerTag);
    }

    public void TakeDamage(int damage, Transform targetTransform)
    {
        hp -= damage;

        float angle = Mathf.Acos(Vector3.Dot(transform.forward, targetTransform.forward)) * Mathf.Rad2Deg;

        if (90f < angle && angle <= 180f) // 90 ~ 180 ��
            animator.SetBool("FrontHit", true);
        else if (0 < angle && angle <= 90f) // 0 ~ 90 ��
            animator.SetBool("FrontHit", false);

        Debug.Log(Mathf.Acos(Vector3.Dot(transform.forward, targetTransform.forward)) * Mathf.Rad2Deg);
        // if (damage >= 15)
        //     animator.SetTrigger("HitDown");
        // else if (damage >= 10)
        //     animator.SetTrigger("HeavyHit");
        // else if (damage >= 5)
        //     animator.SetTrigger("Hit");
        Vector3 pos = transform.position;
        pos.y += 1f;
        string bloodTag = "Blood" + UnityEngine.Random.Range(1, 4).ToString();
        ObjectPooling.Instance.PopObject(bloodTag, pos);
        katana.KatanaTrailActive(false);
        katana.AttackColliderActive(false);
        katana.ParryingColliderActive(false);
        animator.ResetTrigger("ParryAttack");
    }

    public void HitEffect(Vector3 position, Quaternion rotation)
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour, IDamable
{
    private int comboAttackCount { get; set; } = 0;

    [SerializeField] public Transform camTransform;
    [SerializeField] bool autoTargetting = true;
    [SerializeField] private Transform bodyPoint;
    public Vector3 BodyPoint { get { return bodyPoint.position; } }

    [SerializeField] public float moveSpeed { get; } = 5;
    [SerializeField] public float rotSpeed { get; } = 5;
    [SerializeField] public int attackDamage { get; } = 5;
    [SerializeField] private int maxHp = 100;

    [SerializeField] private AudioClip[] audios;
    private AudioSource audioSource;

    public enum AudioTag { DEAD, HIT, FARRY, SWORD }

    public void SoundPlay(AudioTag tag)
    {
        audioSource.clip = audios[(int)tag];
        audioSource.Play();
    }

    private int hp = 10;
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            TransHpEvent?.Invoke(hp, maxHp);
        }
    }

    private ITargetable attackTarget;
    private Transform targetTransform = null;
    public Transform TargetTransform { get { return targetTransform; } }
    private FindTargetOfOverlapSphere findTarget;
    private Coroutine distorCo;
    private Animator animator;

    public UnityAction deadEvent;
    public UnityAction deadReturnEvent;

    private bool isDead = false;
    public bool IsDead
    {
        get => isDead;
        set 
        { 
            isDead = value;
            if (value)
                deadEvent?.Invoke();
        }
    }


    public Action parringAction;
    [HideInInspector] public Katana katana;

    public UnityAction<int, int> TransHpEvent;

    ITargetable preTarget;

    public void DeadReturn()
    {
        animator.SetBool("Dead", isDead = false);
        deadReturnEvent?.Invoke();
        Hp = maxHp;
    }

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
        float curTargetDist = 100f;

        foreach (var coll in colls)
        {
            target = coll.GetComponentInParent<ITargetable>();

            if (!target.IsTarget())
                continue;

            float dist = Vector3.Distance(transform.position, coll.transform.position);

            if (curTargetDist > dist)
            {
                targetTransform = coll.gameObject.transform;
                attackTarget = target;
                curTargetDist = dist;
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
        audioSource = GetComponent<AudioSource>();
        hp = maxHp;
    }

    public void ParryAttackGo()
    {
        animator.SetTrigger("ParryAttack");
    }
    [SerializeField] private Transform bossCo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            FindTarget();
        if (attackTarget != null && !attackTarget.IsTarget())
        {
            preTarget?.NonTarget();
            attackTarget = preTarget = null;
            targetTransform = null;
        }

        Collider[] colls = findTarget.FindTarget();

        if (0 == colls.Length)
        {
            preTarget?.NonTarget();
            attackTarget = preTarget = null;
            targetTransform = null;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (autoTargetting && attackTarget == null)
            FindTarget();
    }

    public void HitTrigger(string triggerTag)
    {
        if (IsDead)
            return;
        animator.SetTrigger(triggerTag);
    }

    public void TakeDamage(int damage, Transform targetTransform)
    {
        if (IsDead)
            return;

        Hp -= damage;

        SoundPlay(AudioTag.HIT);
        

        if (Hp <= 0)
        {

            animator.SetBool("Dead", IsDead = true);
            SoundPlay(AudioTag.DEAD);
        }
        float angle = Mathf.Acos(Vector3.Dot(transform.forward, targetTransform.forward)) * Mathf.Rad2Deg;

        if (90f < angle && angle <= 180f) // 90 ~ 180 ¾Õ
            animator.SetBool("FrontHit", true);
        else if (0 < angle && angle <= 90f) // 0 ~ 90 µÚ
            animator.SetBool("FrontHit", false);

        Vector3 pos = transform.position;
        pos.y += 1f;
        string bloodTag = "Blood" + UnityEngine.Random.Range(1, 4).ToString();
        ObjectPooling.Instance.PopObject(bloodTag, pos);
        katana.KatanaTrailActive(false);
        katana.AttackColliderActive(false);
        katana.ParryingColliderActive(false);
        animator.ResetTrigger("ParryAttack");

       

    }

    public void PlayerKill()
    {
        Hp = 0;
        animator.SetBool("Dead", IsDead = true);
    }

    public void HitEffect(Vector3 position, Quaternion rotation)
    {
        throw new NotImplementedException();
    }
}

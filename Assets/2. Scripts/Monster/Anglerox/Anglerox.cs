using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AngleroxState { IDLE, ATTACK, HIT, DIE, TRACE, JUMP_ATTACK }

public partial class Anglerox : Monster<AngleroxState, Anglerox>
{
    Dictionary<string, AngleroxColliderEvent> colliders = new Dictionary<string, AngleroxColliderEvent>();

    private void Awake()
    {
        Initialize();
        foreach (var coll in GetComponentsInChildren<AngleroxColliderEvent>())
        {

            if (!colliders.ContainsKey(coll.collName))
                colliders.Add(coll.collName, coll);
            else
                Debug.LogError(gameObject.name + "의 콜라이더 " + coll.collName + "이(가) 겹칩니다");
        }

        colliders["Body1"].triggerEnterEvent += BodyColliderEvent;
        colliders["Body2"].triggerEnterEvent += BodyColliderEvent;

        colliders["AttackL"].Init();
        colliders["AttackR"].Init();

        StateInit();
    }
    private void OnEnable()
    {
        colliders["AttackL"].myCollider.enabled = false;
        colliders["AttackR"].myCollider.enabled = false;
        StartCoroutine(DissolveEnable());
        stateMachine.ChangeState(AngleroxState.IDLE);
        isDead = false;
        curHp = maxHP;
    }

    public void AttackColliderEvent(Collider other, Collider thisCollider)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        other.gameObject.GetComponent<IDamable>().TakeDamage(5, transform);
        target.HitTrigger("Hit");
    }

    public void StrongAttackColliderEvent(Collider other, Collider thisCollider)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        other.gameObject.GetComponent<IDamable>().TakeDamage(5, transform);
        target.HitTrigger("HitDown");
    }

    public void BodyColliderEvent(Collider other, Collider thisCollider)
    {
        Debug.Log("BodyColl Call");
    }
    public override void TakeDamage(int damage, Transform transform = null)
    {
        if (curHp <= 0)
            return;

        curHp -= damage;
        colliders["AttackL"].myCollider.enabled = false;
        colliders["AttackR"].myCollider.enabled = false;
        if (curHp > 0)
        {
            ChangeState(AngleroxState.HIT);
            animator.SetTrigger("Hit");
        }
        else
        {
            StopAllCoroutines();
            animator.SetTrigger("Dead");
            targetedObject?.SetActive(false);
            isDead = true;
            ChangeState(AngleroxState.DIE);
        }

    }
    public override void HitEffect(Vector3 position, Quaternion rotaiton)
    {
        string bloodTag = "Blood" + UnityEngine.Random.Range(1, 4).ToString();
        ObjectPooling.Instance.PopObject(bloodTag, position, rotaiton);
    }

    private void StateInit()
    {
        stateMachine = new StateMachine<AngleroxState, Anglerox>(this);
        stateMachine.AddState(AngleroxState.IDLE, new IdleState());
        stateMachine.AddState(AngleroxState.TRACE, new TraceState());
        stateMachine.AddState(AngleroxState.HIT, new HitState());
        stateMachine.AddState(AngleroxState.ATTACK, new AttackState());
        stateMachine.AddState(AngleroxState.DIE, new DieState());
        stateMachine.AddState(AngleroxState.JUMP_ATTACK, new JumpAttackState());
    }

    public void AttackStart(string collName)
    {
        colliders[collName].myCollider.enabled = true;
    }
    public void AttackEnd(string collName)
    {
        colliders[collName].myCollider.enabled = false;
    }
    public void DeadEffect()
    {
        StartCoroutine(DissolveDisable(0.3f));

    }
}

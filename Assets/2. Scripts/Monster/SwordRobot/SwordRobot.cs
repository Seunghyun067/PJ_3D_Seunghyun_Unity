using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordRobotState { IDLE, TRACE, ATTACK, HIT, DIE, NONE_STATE }

public partial class SwordRobot : Monster<SwordRobotState, SwordRobot>
{
    // Start is called before the first frame update
    [SerializeField] private Collider attackCollider;

    public void AttackColliderActive(bool isActive)
    {
        attackCollider.enabled = isActive;
    }

    private void Awake()
    {
        Initialize();

        if (myRenderer.Length == 0)
            myRenderer = GetComponentsInChildren<Renderer>();
        StateInit();
    }

    

    public void DeadEffect()
    {
        StartCoroutine(DissolveDisable(0.3f));

    }

    private void OnEnable()
    {
        StartCoroutine(DissolveEnable());
        stateMachine.ChangeState(SwordRobotState.IDLE);
        isDead = false;
        curHp = maxHP;
    }

    public override void TakeDamage(int damage)
    {
        if (curHp <= 0)
            return;

        curHp -= damage;
        attackCollider.enabled = false;
        if (curHp > 0)
        {
            ChangeState(SwordRobotState.HIT);
            animator.SetTrigger("Hit");
        }
        else
        {
            StopAllCoroutines();
            animator.SetTrigger("Dead");
            targetedObject?.SetActive(false);
            isDead = true;
            ChangeState(SwordRobotState.DIE);
        }
            
    }

    private void StateInit()
    {
        stateMachine = new StateMachine<SwordRobotState, SwordRobot>(this);
        stateMachine.AddState(SwordRobotState.IDLE, new IdleState());
        stateMachine.AddState(SwordRobotState.TRACE, new TraceState());
        stateMachine.AddState(SwordRobotState.HIT, new HitState());
        stateMachine.AddState(SwordRobotState.ATTACK, new AttackState());
        stateMachine.AddState(SwordRobotState.DIE, new DieState());
    }
}

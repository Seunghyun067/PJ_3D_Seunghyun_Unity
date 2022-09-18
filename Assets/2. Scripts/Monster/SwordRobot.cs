using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordRobotState { IDLE, TRACE, ATTACK, HIT, DIE, NONE_STATE }

public partial class SwordRobot : Monster<SwordRobotState, SwordRobot>
{
    // Start is called before the first frame update
    [SerializeField] private Renderer[] myRenderer;
    

    private void Awake()
    {
        Initialize();

        if (myRenderer.Length == 0)
            myRenderer = GetComponentsInChildren<Renderer>();
        StateInit();
    }

    IEnumerator DissolveEnable()
    {
        float dissolveValue = 1f;

        while (dissolveValue > 0f)
        {
            for (int i = 0; i < myRenderer.Length; ++i)
                myRenderer[i].material.SetFloat("_Dissolve", dissolveValue -= Time.deltaTime);
            yield return null;
        }
        for (int i = 0; i < myRenderer.Length; ++i)
            myRenderer[i].material.SetFloat("_Dissolve", 0f);
        yield return null;
    }
    IEnumerator DissolveDisable()
    {
        float dissolveValue = 0f;

        while (dissolveValue < 1f)
        {
            for (int i = 0; i < myRenderer.Length; ++i)
                myRenderer[i].material.SetFloat("_Dissolve", dissolveValue += Time.deltaTime * 0.5f);
            yield return null;
        }
        for (int i = 0; i < myRenderer.Length; ++i)
            myRenderer[i].material.SetFloat("_Dissolve", 1f);
        Destroy(gameObject);
        yield return null;
    }

    public void DeadEffect()
    {
        StartCoroutine(DissolveDisable());

    }

    private void OnEnable()
    {
        Debug.Log("asd");
        StartCoroutine(DissolveEnable());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            animator.SetTrigger("Hit");


    }

    public override void TakeDamage(int damage)
    {
        if (hp <= 0)
            return;

        hp -= damage;       

        if (hp > 0)
        {
            ChangeState(SwordRobotState.HIT);
            animator.SetTrigger("Hit");
        }
        else
        {
            ChangeState(SwordRobotState.DIE);
            animator.SetTrigger("Dead");
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
        stateMachine.ChangeState(SwordRobotState.IDLE);
    }
}

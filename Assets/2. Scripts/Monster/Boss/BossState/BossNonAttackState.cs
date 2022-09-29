using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNonAttackState : BossBaseState
{
    [SerializeField] private float attackDelayTime = 5f;
    private float curDelayTime = 0f;
    [SerializeField] private int curAttackCount = 0;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        curDelayTime = attackDelayTime;
}
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dist = Vector3.Distance(boss.target.transform.position, boss.transform.position);

        if (dist >= 15f)
            return;

        if (dist <= 11f)
        {
            animator.SetTrigger("NearAttack");
        }

        curDelayTime -= Time.deltaTime;
        if (curDelayTime < 0f)
        {
            if (curAttackCount == 2)
            {
                animator.SetTrigger("Hit");
                curAttackCount = 0;
                curDelayTime = attackDelayTime;
                return;
            }
            animator.SetTrigger("Attack");
            curDelayTime = attackDelayTime;
            curAttackCount++;
        }

    }
}
